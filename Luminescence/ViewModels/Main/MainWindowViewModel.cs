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
    
    private double _width = 0;
    private double _height = 0;

    private readonly ExpUsbDeviceService _expDeviceUsbService;

    public MainWindowViewModel(ExpUsbDeviceService expDeviceUsbService)
    {
        _expDeviceUsbService = expDeviceUsbService;

        // _expDeviceUsbService.ConnectDevice();
    }

    // private void ConnectDevice()
    // {
    //     while (true)
    //     {
    //         
    //     }
    // }
}