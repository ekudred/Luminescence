using System;
using System.Reactive;
using System.Reactive.Linq;
using Luminescence.Dialog;
using Luminescence.Services;
using Luminescence.Views;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ToolBarViewModel : BaseViewModel
{
    public ReactiveCommand<Unit, Unit> OpenOptionsDialogCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleActiveCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleConnectCommand { get; }

    public bool PlayEnabled
    {
        get => _playEnabled;
        set => this.RaiseAndSetIfChanged(ref _playEnabled, value);
    }

    public bool StopEnabled
    {
        get => _stopEnabled;
        set => this.RaiseAndSetIfChanged(ref _stopEnabled, value);
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

    public bool InProcess
    {
        get => _inProcess;
        set => this.RaiseAndSetIfChanged(ref _inProcess, value);
    }

    private bool _playEnabled;
    private bool _stopEnabled;
    private string _connectionStatus;
    private bool _connected;
    private bool _inProcess;

    private readonly DialogService _dialogService;
    private readonly ExpDeviceService _expDeviceService;

    public ToolBarViewModel(
        DialogService dialogService,
        ExpDeviceService expDeviceService
    )
    {
        _dialogService = dialogService;
        _expDeviceService = expDeviceService;

        Observable.CombineLatest(_expDeviceService.InProcess, _expDeviceService.Connected)
            .Subscribe(result =>
            {
                Connected = result[1];
                InProcess = result[0];
            });

        this.WhenAnyValue(x => x.Connected, x => x._inProcess)
            .Subscribe(result =>
            {
                var (connected, inProcess) = result;

                PlayEnabled = connected && !inProcess;
                StopEnabled = connected && inProcess;

                Connected = connected;
                ConnectionStatus = GetUsbConnectionStatus(Connected);
            });

        OpenOptionsDialogCommand = ReactiveCommand.Create(OpenOptionsDialog);
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
        ToggleConnectCommand = ReactiveCommand.Create(ToggleConnect);
    }

    public void OpenOptionsDialog()
    {
        _dialogService.ShowDialog(new OptionsDialog());
    }

    public void ToggleActive()
    {
        if (!InProcess)
        {
            _expDeviceService.RunProcess();

            return;
        }

        _expDeviceService.StopProcess();
    }

    public void ToggleConnect()
    {
        if (!Connected)
        {
            _expDeviceService.Connect();

            return;
        }

        _expDeviceService.Disconnect();
    }

    private string GetUsbConnectionStatus(bool connected) =>
        connected ? "Устройство подключено" : "Подключить устройство";
}