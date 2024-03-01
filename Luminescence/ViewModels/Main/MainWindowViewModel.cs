using System;
using Luminescence.Services;
using Luminescence.Views;
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

    public MainWindowViewModel(
        ExpDeviceService expDeviceService,
        HidService hidService
    )
    {
        _expDeviceService = expDeviceService;
        _hidService = hidService;
    }

    public void Initialize()
    {
        _hidService.Init()
            .Subscribe();

        _expDeviceService.RunCheckAvailableDevice();
    }

    public void Destroy()
    {
        _expDeviceService.StopCheckAvailableDevice();
        _expDeviceService.Disconnect();

        _hidService.Exit()
            .Subscribe();
    }
}