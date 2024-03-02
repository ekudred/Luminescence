using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.Dialog;
using Luminescence.Views;

namespace Luminescence.Services;

public class ExpDeviceService : HidDeviceService
{
    public readonly Subject<ExpReadDto> CurrentData = new();
    public readonly Subject<bool> InProcess = new();

    private Subject<bool> _readDataOn;

    private readonly DialogService _dialogService;

    public ExpDeviceService
    (
        HidService hidService,
        DialogService dialogService
    ) : base
    (
        new ExpDeviceOptions(),
        hidService
    )
    {
        _dialogService = dialogService;

        СonnectionLost
            .Merge(InProcess)
            // .Select(_ => InProcess)
            // .Switch()
            .Where(inProcess => inProcess)
            .Subscribe(_ => { InProcess.OnNext(false); });

        // ReadException
        //     .Subscribe(_ => _dialogService.ShowDialog(new FailDialog()).Subscribe());
    }

    public void RunProcess()
    {
        HidService.Write(DeviceHandle, ExpWriteDto.RunDto.ToBytes())
            .Subscribe(
                _ =>
                {
                    _readDataOn = new();

                    ReadData
                        .TakeUntil(_readDataOn)
                        .Select(ExpReadDto.FromBytes)
                        .Subscribe(dto =>
                        {
                            InProcess.OnNext(dto.Mode == 0x1);
                            CurrentData.OnNext(dto);
                        });
                }
                // _ => { },
                // () => { _dialogService.ShowDialog(new FailDialog()); }
            );
    }

    public void StopProcess()
    {
        HidService.Write(DeviceHandle, ExpWriteDto.StopDto.ToBytes())
            .Subscribe(
                _ =>
                {
                    _readDataOn.OnNext(true);
                    _readDataOn.OnCompleted();
                    _readDataOn = null;

                    InProcess.OnNext(false);
                }
                // _ => { },
                // () => { _dialogService.ShowDialog(new FailDialog()); }
            );
    }

    public void SendData(ExpWriteDto dto)
    {
        HidService.Write(DeviceHandle, dto.ToBytes())
            .Subscribe(
                // _ => { }
                // _ => { },
                // () => { _dialogService.ShowDialog(new FailDialog()); }
            );
    }
}