using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.Usb;
using Luminescence.Utils;

namespace Luminescence.Services;

public class HidDeviceService
{
    public readonly BehaviorSubject<bool> Connected = new(false);

    protected nint DeviceHandle;

    protected readonly Subject<byte[]> ReadData = new();
    protected readonly Subject<Exception> ReadException = new();
    protected readonly Subject<bool> СonnectionLost = new();
    protected readonly HidService HidService;

    private bool _opened => DeviceHandle != IntPtr.Zero;

    private Subject<int> _listenDeviceOn;
    private Subject<int> _checkOn;

    private readonly IHidDeviceOptions _options;

    // test
    public bool TestActive = false;
    public readonly bool IsTest = false;
    // end test

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
        if (IsTest)
        {
            Connect();

            return;
        }
        // end test
        if (_checkOn != null)
        {
            return;
        }

        _checkOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.CheckInterval))
            .Select(_ => HidService.Enumerate(_options.VendorId, _options.ProductId)).Switch()
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
        if (IsTest)
        {
            Connected.OnNext(true);

            return;
        }
        // end test
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
        if (IsTest)
        {
            return;
        }
        // end test

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
        if (IsTest)
        {
            if (_listenDeviceOn != null)
            {
                return;
            }

            _listenDeviceOn = new();

            Random random = new Random();
            double counter = 0;

            Observable
                .Interval(TimeSpan.FromMilliseconds(_options.ReadInterval))
                .Where(_ => TestActive)
                .TakeUntil(_listenDeviceOn)
                .Subscribe(_ =>
                {
                    var temperature = Math.Truncate(random.NextSingle() * 1000);
                    var intensity = Math.Truncate(random.NextDouble() * 500);
                    var LEDCurrent = Math.Truncate(random.NextDouble());

                    var structure = new ExpReadDto();
                    structure.Counter += UInt32.Parse(counter.ToString());
                    structure.Temperature = float.Parse(temperature.ToString());
                    structure.Intensity = UInt32.Parse(intensity.ToString());
                    structure.LEDCurrent = UInt32.Parse(LEDCurrent.ToString());

                    ReadData.OnNext(StructUtil.StructToBytes(structure));

                    counter += 1;
                });

            return;
        }
        // end test

        if (!_opened || _listenDeviceOn != null)
        {
            return;
        }

        _listenDeviceOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.ReadInterval))
            .Select(_ => HidService.Read(DeviceHandle, _options.ReadReportLength)).Switch()
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