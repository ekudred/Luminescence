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

    public MainWindowViewModel(
        ExpDeviceService expDeviceService,
        MainWindowProvider mainWindowProvider,
        HidService hidService
    )
    {
        expDeviceService.RunCheckAvailableDevice();

        // mainWindowProvider.GetMainWindow().Closing += (_, _) =>
        // {
        //     hidService.Exit()
        //         .Subscribe();
        // };
    }
}