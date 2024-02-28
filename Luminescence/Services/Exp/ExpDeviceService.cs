using System;
using System.Reactive.Subjects;
using Luminescence.Dialog;
using Luminescence.Views;

namespace Luminescence.Services;

public class ExpDeviceService : HidDeviceService
{
    public readonly Subject<ExpReadData> CurrentData = new();
    public readonly Subject<bool> InProcess = new();

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

        ReadErrorEvent += (_, args) => { _dialogService.ShowDialog(new FailDialog()); };
    }

    public void RunProcess()
    {
        HidService.Write(DeviceHandle, ExpWriteData.RunData.ToBytes())
            .Subscribe(
                _ =>
                {
                    ReadReportArrivedEvent += (_, x) => { CurrentData.OnNext(ExpReadData.FromBytes(x.Data)); };
                    InProcess.OnNext(true);
                },
                () => { _dialogService.ShowDialog(new FailDialog()); }
            );
    }

    public void StopProcess()
    {
        HidService.Write(DeviceHandle, ExpWriteData.StopData.ToBytes())
            .Subscribe(
                _ => { InProcess.OnNext(false); },
                () => { _dialogService.ShowDialog(new FailDialog()); }
            );
    }

    public void SendData(ExpWriteData data)
    {
        HidService.Write(DeviceHandle, data.ToBytes())
            .Subscribe(
                _ => { },
                () => { _dialogService.ShowDialog(new FailDialog()); }
            );
    }
}