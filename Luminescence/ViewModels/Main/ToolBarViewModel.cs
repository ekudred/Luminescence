using System;
using System.Reactive;
using Luminescence.Dialog;
using Luminescence.Enums;
using Luminescence.Models;
using Luminescence.Services;
using Luminescence.Views;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ToolBarViewModel : BaseViewModel
{
    public ReactiveCommand<Unit, Unit> OpenOptionsDialogCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleActiveCommand { get; }
    public ReactiveCommand<Unit, Unit> ConnectCommand { get; }

    public bool Active
    {
        get => _active;
        set => this.RaiseAndSetIfChanged(ref _active, value);
    }

    public string ConnectionStatus
    {
        get => _connectionStatus;
        set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
    }

    public bool Connected
    {
        get => _connected;
        set => this.RaiseAndSetIfChanged(ref _connected, value);
    }

    private bool _active;
    private string _connectionStatus;
    private bool _connected;

    private readonly DialogService _dialogService;
    private readonly ExpUsbDeviceService _expDeviceUsbService;

    public ToolBarViewModel(
        DialogService dialogService,
        ExpUsbDeviceService expDeviceUsbService
    )
    {
        _dialogService = dialogService;
        _expDeviceUsbService = expDeviceUsbService;

        this.WhenAnyValue(x => x._expDeviceUsbService.Active)
            .Subscribe(active => { Active = active; });
        this.WhenAnyValue(x => x._expDeviceUsbService.ConnectionStatusCode)
            .Subscribe(connectionStatusCode =>
            {
                ConnectionStatus = GetUsbConnectionStatus(connectionStatusCode);
                Connected = connectionStatusCode == UsbConnectionStatusCode.Connected;
            });

        OpenOptionsDialogCommand = ReactiveCommand.Create(OpenOptionsDialog);
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
        ConnectCommand = ReactiveCommand.Create(Connect);
    }

    public void OpenOptionsDialog()
    {
        // _dialogService.ShowDialog(new FailDialog());
        _dialogService.ShowDialog(new OptionsDialog());
    }

    public void ToggleActive()
    {
        if (!_expDeviceUsbService.Active)
        {
            _expDeviceUsbService.StartScanDevice();

            return;
        }

        _expDeviceUsbService.StopScanDevice();
    }

    public void Connect()
    {
        _expDeviceUsbService.ConnectDevice();
    }

    private string GetUsbConnectionStatus(UsbConnectionStatusCode status)
    {
        switch (status)
        {
            case UsbConnectionStatusCode.Connected:
                return "Устройство подключено";
            case UsbConnectionStatusCode.NoConnection:
                return "Подключить устройство";
            default:
                return "";
        }
    }
}