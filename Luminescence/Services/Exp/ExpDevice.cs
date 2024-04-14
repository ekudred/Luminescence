using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.Dialog;
using Luminescence.UsbHid;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class ExpDevice : UsbHidDevice
{
    public readonly Subject<ExpReadDto> CurrentData = new();
    public readonly BehaviorSubject<bool> InProcess = new(false);

    private Subject<bool> _readDataOn;

    private readonly DialogBaseService _dialogBaseService;

    public ExpDevice
    (
        UsbHidService hidService,
        DialogBaseService dialogBaseService
    ) : base
    (
        new ExpDeviceOptions(),
        hidService
    )
    {
        _dialogBaseService = dialogBaseService;

        if (IsTest)
        {
            return;
        }

        СonnectionLost
            .Merge(InProcess)
            .Where(inProcess => inProcess)
            .Subscribe(_ => { InProcess.OnNext(false); });

        // ReadException
        //     .Subscribe(_ => _dialogService.ShowDialog("ErrorDialog").Subscribe());
    }

    public void RunProcess(ExpWriteDto dto)
    {
        if (IsTest)
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
                _ => { _dialogBaseService.Create<ErrorDialogViewModel>(); }
            );
    }

    public void StopProcess()
    {
        if (IsTest)
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
                _ => { _dialogBaseService.Create<ErrorDialogViewModel>(); }
            );
    }
}