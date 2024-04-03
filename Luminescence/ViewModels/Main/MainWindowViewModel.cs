using System;
using System.Reactive;
using System.Windows.Input;
using Luminescence.Dialog;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    public double Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    public double Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }

    public ICommand OpenCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand OpenSettingsDialogCommand { get; }

    private double _width;
    private double _height;

    private readonly ExpDeviceService _expDeviceService;
    private readonly ExpChartService _expChartService;
    private readonly HidService _hidService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;
    private readonly MeasurementSettingsFormViewModel _measurementSettingsFormViewModel;

    public MainWindowViewModel(
        ExpDeviceService expDeviceService,
        ExpChartService expChartService,
        HidService hidService,
        MeasurementSettingsFormService measurementSettingsFormService,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel,
        DialogService dialogService
    )
    {
        _expDeviceService = expDeviceService;
        _expChartService = expChartService;
        _hidService = hidService;
        _measurementSettingsFormService = measurementSettingsFormService;
        _measurementSettingsFormViewModel = measurementSettingsFormViewModel;

        OpenCommand = ReactiveCommand.Create<Unit>(_ => _expChartService.Open());
        SaveCommand = ReactiveCommand.Create<Unit>(_ => _expChartService.Save());
        OpenSettingsDialogCommand =
            ReactiveCommand.Create<Unit>(_ =>
            {
                if (!_expDeviceService.InProcess.Value)
                {
                    dialogService.Create<SettingsDialogViewModel>().Open();
                }
            });
    }

    public void Initialize()
    {
        _hidService.Init()
            .Subscribe();
        _expDeviceService.RunAvailableDeviceCheck();
        _measurementSettingsFormService.Initialize(_measurementSettingsFormViewModel);
    }

    public void Destroy()
    {
        _expDeviceService.StopAvailableDeviceCheck();
        _expDeviceService.Disconnect();
        _hidService.Exit()
            .Subscribe();
        _measurementSettingsFormViewModel.Destroy();
    }
}