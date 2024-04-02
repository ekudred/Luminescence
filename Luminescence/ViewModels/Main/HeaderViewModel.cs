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
    public ICommand OpenCommand { get; }
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

    public bool OpenEnabled
    {
        get => _openEnabled;
        set => this.RaiseAndSetIfChanged(ref _openEnabled, value);
    }

    public bool SaveEnabled
    {
        get => _saveEnabled;
        set => this.RaiseAndSetIfChanged(ref _saveEnabled, value);
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
    private bool _openEnabled;
    private bool _saveEnabled;
    private bool _connected;
    private bool _inProcess;

    private readonly DialogService _dialogService;
    private readonly ExpDeviceService _expDeviceService;
    private readonly ExpChartService _expChartService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;

    public HeaderViewModel(
        DialogService dialogService,
        ExpDeviceService expDeviceService,
        ExpChartService expChartService,
        MeasurementSettingsFormService measurementSettingsFormService
    )
    {
        _dialogService = dialogService;
        _expDeviceService = expDeviceService;
        _expChartService = expChartService;
        _measurementSettingsFormService = measurementSettingsFormService;

        _expDeviceService.Connected
            .Subscribe(connected => { Connected = connected; });
        _expDeviceService.InProcess
            .Subscribe(inProcess =>
            {
                InProcess = inProcess;

                OpenEnabled = !InProcess;
                SaveEnabled = !InProcess && _expChartService.Data.Count > 0;
            });

        this.WhenAnyValue(x => x.Connected, x => x.InProcess)
            .Subscribe(result =>
            {
                var (connected, inProcess) = result;

                PlayEnabled = connected && !inProcess;
                StopEnabled = connected && inProcess;

                Connected = connected;
            });

        OpenSettingsDialogCommand =
            ReactiveCommand.Create<Unit>(_ => _dialogService.Create<SettingsDialogViewModel>().Open());
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
        OpenCommand = ReactiveCommand.Create<Unit>(_ => _expChartService.Open());
        SaveCommand = ReactiveCommand.Create<Unit>(_ => _expChartService.Save());
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
}