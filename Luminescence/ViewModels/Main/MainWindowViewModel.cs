using System;
using System.Reactive;
using System.Windows.Input;
using Luminescence.Services;
using Luminescence.Shared.UsbHid;
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

    private readonly ExpDevice _expDevice;
    private readonly UsbHidService _usbHidService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;
    private readonly MeasurementSettingsFormViewModel _measurementSettingsFormViewModel;

    public MainWindowViewModel(
        ExpDevice expDevice,
        ExpChartService expChartService,
        UsbHidService usbHidService,
        MeasurementSettingsFormService measurementSettingsFormService,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel,
        DialogService dialogService
    )
    {
        _expDevice = expDevice;
        _usbHidService = usbHidService;
        _measurementSettingsFormService = measurementSettingsFormService;
        _measurementSettingsFormViewModel = measurementSettingsFormViewModel;

        OpenCommand = ReactiveCommand.Create<Unit>(_ => expChartService.Open());
        SaveCommand = ReactiveCommand.Create<Unit>(_ => expChartService.Save());
        OpenSettingsDialogCommand =
            ReactiveCommand.Create<Unit>(_ =>
            {
                if (!_expDevice.InProcess.Value)
                {
                    dialogService.Create<SettingsDialogViewModel>().Open();
                }
            });
    }

    public void Initialize()
    {
        _usbHidService.Init()
            .Subscribe();
        _expDevice.RunAvailableDeviceCheck();
        _measurementSettingsFormService.Initialize(_measurementSettingsFormViewModel);
    }

    public void Destroy()
    {
        _expDevice.StopAvailableDeviceCheck();
        _expDevice.Disconnect();
        _usbHidService.Exit()
            .Subscribe();
        _measurementSettingsFormViewModel.Destroy();
    }
}