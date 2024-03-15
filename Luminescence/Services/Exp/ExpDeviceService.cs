using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.Dialog;

namespace Luminescence.Services;

public class ExpDeviceService : HidDeviceService
{
    public readonly Subject<ExpReadDto> CurrentData = new();
    public readonly Subject<bool> InProcess = new();

    private Subject<bool> _readDataOn = new();

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
            .Where(inProcess => inProcess)
            .Subscribe(_ => { InProcess.OnNext(false); });

        // ReadException
        //     .Subscribe(_ => _dialogService.ShowDialog("ErrorDialog").Subscribe());
    }

    public void RunProcess(ExpWriteDto dto)
    {
        var dt2o = new ExpWriteDto();

        HidService.Write(DeviceHandle, dt2o.GetRunDto().ToBytes())
            .Subscribe(
                _ =>
                {
                    ReadData
                        .Throttle(TimeSpan.FromMilliseconds(1000))
                        .Select(ExpReadDto.FromBytes)
                        .TakeUntil(_readDataOn)
                        .Subscribe(dto =>
                        {
                            InProcess.OnNext(dto.Mode == 0x1);
                            CurrentData.OnNext(dto);
                        });
                }
                // _ => { },
                // () => { _dialogService.ShowDialog("ErrorDialog"); }
            );
    }

    public void StopProcess()
    {
        HidService.Write(DeviceHandle, ExpWriteDto.StopDto.ToBytes())
            .Subscribe(
                _ =>
                {
                    _readDataOn.OnNext(true);
                    // _readDataOn.OnCompleted();
                    // _readDataOn = null;

                    InProcess.OnNext(false);
                }
                // _ => { },
                // () => { _dialogService.ShowDialog("ErrorDialog"); }
            );
    }
}