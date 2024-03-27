using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Luminescence.Dialog;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class HeaderViewModel : BaseViewModel
{
    public ICommand OpenSettingsDialogCommand { get; }
    public ICommand ToggleActiveCommand { get; }
    public ICommand SaveCommand { get; }

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
    private readonly SystemDialogService _systemDialogService;
    private readonly FileService _fileService;
    private readonly ExpDeviceService _expDeviceService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;

    public HeaderViewModel(
        DialogService dialogService,
        SystemDialogService systemDialogService,
        FileService fileService,
        ExpDeviceService expDeviceService,
        MeasurementSettingsFormService measurementSettingsFormService
    )
    {
        _dialogService = dialogService;
        _systemDialogService = systemDialogService;
        _fileService = fileService;
        _expDeviceService = expDeviceService;
        _measurementSettingsFormService = measurementSettingsFormService;

        _expDeviceService.Connected
            .Subscribe(connected => { Connected = connected; });
        _expDeviceService.InProcess
            .Subscribe(inProcess => { InProcess = inProcess; });

        // Observable.Zip(_expDeviceService.InProcess, _expDeviceService.Connected)
        //     .Subscribe(result =>
        //     {
        //         Connected = result[1];
        //         InProcess = result[0];
        //     });

        this.WhenAnyValue(x => x.Connected, x => x.InProcess)
            .Subscribe(result =>
            {
                var (connected, inProcess) = result;

                PlayEnabled = connected && !inProcess;
                StopEnabled = connected && inProcess;

                Connected = connected;
                ConnectionStatus = GetUsbConnectionStatus(Connected);
            });

        OpenSettingsDialogCommand = ReactiveCommand.Create(OpenSettingsDialog);
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
        SaveCommand = ReactiveCommand.Create(Save);
    }

    private void OpenSettingsDialog()
    {
        var dialog = _dialogService.Create<SettingsDialogViewModel>();

        _systemDialogService.UseConfirm(dialog,
            new ConfirmationDialogParam("Вы уверены, что не хотите применить изменения?"));

        dialog.Open();
    }

    private void ToggleActive()
    {
        if (!InProcess)
        {
            _measurementSettingsFormService.GetModelFromStorage()
                .Where(model => model != null)
                .Subscribe(model => { _expDeviceService.RunProcess(model.ToDto()); });

            return;
        }

        _expDeviceService.StopProcess();
    }

    private void Save()
    {
        _fileService.Save()
            .Subscribe();
    }

    private string GetUsbConnectionStatus(bool connected) =>
        connected ? "Устройство подключено" : "Устройство не подключено";
}