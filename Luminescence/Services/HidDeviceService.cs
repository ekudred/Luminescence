using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.Usb;

namespace Luminescence.Services;

public class HidDeviceService
{
    public readonly Subject<bool> Connected = new();

    protected nint DeviceHandle;

    protected readonly Subject<byte[]> ReadData = new();
    protected readonly Subject<Exception> ReadException = new();
    protected readonly Subject<bool> СonnectionLost = new();
    protected readonly HidService HidService;

    private bool _opened => DeviceHandle != IntPtr.Zero;

    private Subject<int> _listenDeviceOn;
    private Subject<int> _checkOn;

    private readonly IHidDeviceOptions _options;

    protected HidDeviceService(
        IHidDeviceOptions options,
        HidService hidService
    )
    {
        _options = options;
        HidService = hidService;
    }

    public void RunAvailableDeviceCheck()
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
                        СonnectionLost.OnNext(true);
                        Disconnect();
                    }
                }
            );
    }

    public void StopAvailableDeviceCheck()
    {
        if (_checkOn == null)
        {
            return;
        }

        _checkOn.OnNext(0);
        _checkOn.OnCompleted();
        _checkOn = null;
    }

    public void Connect()
    {
        ConnectDevice();
        StartListenDevice();
    }

    public void Disconnect()
    {
        DisconnectDevice();
        StopListenDevice();
    }

    private void ConnectDevice()
    {
        if (_opened)
        {
            return;
        }

        HidService.Open(_options.VendorId, _options.ProductId, _options.SerialNumber)
            .Subscribe(
                deviceHandle =>
                {
                    DeviceHandle = deviceHandle;
                    Connected.OnNext(_opened);
                },
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
        Connected.OnNext(_opened);
    }

    private void StartListenDevice()
    {
        if (!_opened || _listenDeviceOn != null)
        {
            return;
        }

        _listenDeviceOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.ReadInterval))
            .Select(_ => HidService.Read(DeviceHandle, _options.ReadReportLength))
            .Switch()
            .TakeUntil(_listenDeviceOn)
            .Subscribe(
                data => { ReadData.OnNext(data); },
                exception => { ReadException.OnNext(exception); }
            );
    }

    private void StopListenDevice()
    {
        if (_listenDeviceOn == null)
        {
            return;
        }

        _listenDeviceOn.OnNext(0);
        _listenDeviceOn.OnCompleted();
        _listenDeviceOn = null;
    }
}