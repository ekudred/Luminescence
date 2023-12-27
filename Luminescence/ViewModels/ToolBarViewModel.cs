using System.Reactive;
using Luminescence.Dialog;
using Luminescence.Usb;
using Luminescence.Views;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ToolBarViewModel : BaseViewModel
{
    public ReactiveCommand<Unit, Unit> OpenOptionsDialogCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleActiveCommand { get; }

    public UsbConnectionStatusCode ConnectionStatusCode
    {
        get => _connectionStatusCode;
        set
        {
            ConnectionStatus = GetUsbConnectionStatus(_connectionStatusCode);

            this.RaiseAndSetIfChanged(ref _connectionStatusCode, value);
        }
    }

    public string ConnectionStatus
    {
        get => _connectionStatus;
        set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
    }

    public bool IsActive
    {
        get => _isActive;
        set => this.RaiseAndSetIfChanged(ref _isActive, value);
    }

    private UsbConnectionStatusCode _connectionStatusCode = UsbConnectionStatusCode.NoConnection;
    private string _connectionStatus = GetUsbConnectionStatus(UsbConnectionStatusCode.NoConnection);
    private bool _isActive = false;

    private DialogService _dialogService;

    public ToolBarViewModel(DialogService dialogService)
    {
        _dialogService = dialogService;

        OpenOptionsDialogCommand = ReactiveCommand.Create(OpenOptionsDialog);
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
    }

    public void ToggleActive()
    {
        IsActive = !IsActive;
    }

    public void OpenOptionsDialog()
    {
        _dialogService.ShowDialog(new OptionsDialog());
    }

    private static string GetUsbConnectionStatus(UsbConnectionStatusCode status)
    {
        switch (status)
        {
            case UsbConnectionStatusCode.Connected:
                return "Устройство подключено";
            case UsbConnectionStatusCode.Connecting:
                return "Идёт подключение...";
            case UsbConnectionStatusCode.NoConnection:
                return "Нет подключенного устройства";
            default:
                return "";
        }
    }
}