using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.Usb;

namespace Luminescence.Services;

public class HidDeviceService
{
    public readonly Subject<bool> Connected = new();

    public event EventHandler<HidReportEventArgs> ReadReportArrivedEvent;
    public event EventHandler<HidReadErrorEventArgs> ReadErrorEvent;

    protected nint DeviceHandle;

    protected readonly HidService HidService;
    protected readonly MainWindowProvider MainWindowProvider;

    private bool _opened => DeviceHandle != IntPtr.Zero;

    private Subject<int>? _readOn;
    private Subject<int>? _checkOn;

    private readonly HidDeviceOptions _options;

    protected HidDeviceService(
        HidDeviceOptions options,
        HidService hidService,
        MainWindowProvider mainWindowProvider
    )
    {
        _options = options;
        HidService = hidService;
        MainWindowProvider = mainWindowProvider;

        HidService.Init()
            .Subscribe();

        // MainWindowProvider.GetMainWindow().Closing += (_, _) =>
        // {
        //     Disconnect();
        //     StopReadDevice();
        // };
    }

    public void RunCheckAvailableDevice()
    {
        if (_checkOn != null)
        {
            return;
        }

        _checkOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.CheckInterval))
            .Select(_ => HidService.Enumerate(_options.VendorId, _options.ProductId))
            .Switch()
            .TakeUntil(_checkOn)
            .Subscribe(
                data =>
                {
                    if (!_opened && data != IntPtr.Zero)
                    {
                        Connect();
                    }

                    if (_opened && data == IntPtr.Zero)
                    {
                        Disconnect();
                    }
                }
            );
    }

    public void StopCheckAvailableDevice()
    {
        if (_checkOn == null)
        {
            return;
        }

        _checkOn.OnCompleted();
        _checkOn = null;
    }

    public void Connect()
    {
        ConnectDevice();
        StartReadDevice();
    }

    public void Disconnect()
    {
        DisconnectDevice();
        StopReadDevice();
    }

    private void ConnectDevice()
    {
        if (_opened)
        {
            return;
        }

        HidService.Open(_options.VendorId, _options.ProductId, _options.SerialNumber)
            .Subscribe(
                deviceHandle => { DeviceHandle = deviceHandle; },
                () => { }
            );
    }

    private void DisconnectDevice()
    {
        if (!_opened)
        {
            return;
        }

        HidService.Close(DeviceHandle)
            .Subscribe();

        DeviceHandle = IntPtr.Zero;
    }

    private void StartReadDevice()
    {
        if (!_opened || _readOn != null)
        {
            return;
        }

        _readOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.ReadInterval))
            .Select(_ => HidService.Read(DeviceHandle, _options.ReadReportLength))
            .Switch()
            .Where(data => data.Length > 0)
            .TakeUntil(_readOn)
            .Subscribe(
                data => ReadReportArrivedEvent(this, new HidReportEventArgs(data)),
                exception => ReadErrorEvent(this, new HidReadErrorEventArgs(exception))
            );
    }

    private void StopReadDevice()
    {
        if (!_opened || _readOn == null)
        {
            return;
        }

        _readOn.OnCompleted();
        _readOn = null;
    }
}