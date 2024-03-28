using System;
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

    private double _width;
    private double _height;

    private readonly ExpDeviceService _expDeviceService;
    private readonly HidService _hidService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;
    private readonly MeasurementSettingsFormViewModel _measurementSettingsFormViewModel;

    public MainWindowViewModel(
        ExpDeviceService expDeviceService,
        HidService hidService,
        MeasurementSettingsFormService measurementSettingsFormService,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel
    )
    {
        _expDeviceService = expDeviceService;
        _hidService = hidService;
        _measurementSettingsFormService = measurementSettingsFormService;
        _measurementSettingsFormViewModel = measurementSettingsFormViewModel;
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