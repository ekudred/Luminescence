using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.UsbHid;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class ExpDevice : UsbHidDevice
{
    public readonly Subject<ExpReadDto> CurrentData = new();
    public readonly BehaviorSubject<bool> InProcess = new(false);

    private Subject<bool> _readDataOn;

    private readonly DialogService _dialogService;

    public ExpDevice
    (
        UsbHidService hidService,
        DialogService dialogService
    ) : base
    (
        new ExpDeviceOptions(),
        hidService
    )
    {
        _dialogService = dialogService;

        if (TestMode)
        {
            return;
        }

        СonnectionLost
            .Merge(InProcess)
            .Where(inProcess => inProcess)
            .Subscribe(_ => { InProcess.OnNext(false); });

        ReadException
            .Select(_ => _dialogService.ShowError()).Switch()
            .Subscribe();
    }

    public void RunProcess(ExpWriteDto dto)
    {
        if (TestMode)
        {
            _readDataOn = new();

            InProcess.OnNext(true);
            TestActive = true;

            ReadData
                .Where(data => data.Length != 0)
                .Select(ExpReadDto.FromBytes)
                .TakeUntil(_readDataOn)
                .Subscribe(dto => { CurrentData.OnNext(dto); });

            return;
        }
        // end test

        UsbHidService.Write(DeviceHandle, dto.GetRunDto().ToBytes())
            .Subscribe(
                _ =>
                {
                    _readDataOn = new();

                    InProcess.OnNext(true);

                    ReadData
                        .Where(data => data.Length != 0)
                        .Select(ExpReadDto.FromBytes)
                        .TakeUntil(_readDataOn)
                        .Subscribe(dto =>
                        {
                            if (dto.Mode == 0x2)
                            {
                                InProcess.OnNext(false);
                            }

                            CurrentData.OnNext(dto);
                        });
                },
                _ => { _dialogService.ShowError().Subscribe(); }
            );
    }

    public void StopProcess()
    {
        if (TestMode)
        {
            _readDataOn.OnNext(true);
            _readDataOn = null;

            InProcess.OnNext(false);
            TestActive = false;

            return;
        }
        // end test

        UsbHidService.Write(DeviceHandle, ExpWriteDto.StopDto.ToBytes())
            .Subscribe(
                _ =>
                {
                    _readDataOn.OnNext(true);
                    _readDataOn = null;

                    InProcess.OnNext(false);
                },
                _ => { _dialogService.ShowError().Subscribe(); }
            );
    }
}