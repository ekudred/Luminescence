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

    private bool _inProcess;

    private Subject<bool> re;

    private readonly DialogService _dialogService;

    public ExpDeviceService
    (
        HidService hidService,
        MainWindowProvider mainWindowProvider,
        DialogService dialogService
    ) : base
    (
        new ExpDeviceOptions(),
        hidService,
        mainWindowProvider
    )
    {
        _dialogService = dialogService;

        DisconnectEvent.Subscribe(_ =>
        {
            if (_inProcess)
            {
                // StopProcess();
                InProcess.OnNext(false);
                _inProcess = false;
            }
        });

        // ReadErrorEvent += (_, args) => { _dialogService.ShowDialog(new FailDialog()); };
    }

    public void RunProcess()
    {
        HidService.Write(DeviceHandle, ExpWriteDto.RunDto.ToBytes())
            .Subscribe(
                _ =>
                {
                    re = new();
                    // ReadReportArrivedEvent += (_, args) => { CurrentData.OnNext(ExpReadDto.FromBytes(args.Data)); };
                    ReadReportArrivedEvent
                        .TakeUntil(re)
                        .Subscribe(args =>
                        {
                            var dto = ExpReadDto.FromBytes(args.Data);

                            if (dto.Mode == 0x1 && !_inProcess)
                            {
                                InProcess.OnNext(true);
                                _inProcess = true;
                            }

                            // if (dto.Mode == 0x14)
                            // {
                            //     StopProcess();
                            // }

                            CurrentData.OnNext(dto);
                        });
                    InProcess.OnNext(true);
                    _inProcess = true;
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
                    re.OnNext(true);
                    re.OnCompleted();
                    re = null;

                    InProcess.OnNext(false);
                    _inProcess = false;
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