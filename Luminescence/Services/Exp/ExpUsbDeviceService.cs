using System;
using Avalonia.Threading;
using Luminescence.Usb;
using Luminescence.Enums;
using Luminescence.Models;
using Luminescence.Utils;
using ReactiveUI;

namespace Luminescence.Services;

public class ExpUsbDeviceService : ReactiveObject
{
    public UsbDevice? Device
    {
        get => _device;
        private set => this.RaiseAndSetIfChanged(ref _device, value);
    }

    public bool Initialized
    {
        get => _initialized;
        private set => this.RaiseAndSetIfChanged(ref _initialized, value);
    }

    public ReadableDataStructure Data
    {
        get => _data;
        private set => this.RaiseAndSetIfChanged(ref _data, value);
    }

    public UsbConnectionStatusCode ConnectionStatusCode
    {
        get => _connectionStatusCode;
        private set
        {
            ConnectionStatus = GetUsbConnectionStatus(_connectionStatusCode);

            this.RaiseAndSetIfChanged(ref _connectionStatusCode, value);
        }
    }

    public string ConnectionStatus
    {
        get => _connectionStatus;
        private set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
    }

    // public List<byte[]> Reports = new List<byte[]>();

    private UsbDevice _device;

    private bool _initialized = false;

    private ReadableDataStructure _data;

    private UsbConnectionStatusCode _connectionStatusCode = UsbConnectionStatusCode.NoConnection;
    private string _connectionStatus = GetUsbConnectionStatus(UsbConnectionStatusCode.NoConnection);

    // public int _reportsCount = 0;

    private DispatcherTimer _timer;

    private readonly int _scanDelay = 1;

    // public void Initialize()
    // {
    //     // App.RegisterHandler(DisposeUSBDevice);
    //     ConnectionStatusCode = UsbConnectionStatusCode.Connecting;
    //
    //     try
    //     {
    //         InitDevice();
    //     }
    //     catch (Exception exception)
    //     {
    //         DestroyDevice();
    //     }
    // }

    // public void Reset()
    // {
    //     byte[] data = new byte[32];
    //     data[0] = 0x01;
    //     data[1] = 0x02;
    //
    //     try
    //     {
    //         Device.Write(data);
    //     }
    //     catch (Exception exception)
    //     {
    //     }
    // }

    public void InitDevice()
    {
        try
        {
            Device = new UsbDevice(0x0483, 0x5750, null, true, 64);
            Device.InputReportArrivedEvent += PullData;
            Device.StartAsyncRead();
            Initialized = true;
            ConnectionStatusCode = UsbConnectionStatusCode.Connected;
            // StartScanDevice();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    public void DestroyDevice()
    {
        if (Device == null)
        {
            return;
        }

        try
        {
            Device.StopAsyncRead();
            Device.Dispose();
            Device = null;
            Initialized = false;
            ConnectionStatusCode = UsbConnectionStatusCode.NoConnection;
            // StopScanDevice();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    public void PushData(WritableDataStructure data)
    {
        try
        {
            Device.Write(StructUtil.StructToByte(data));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void StartScanDevice()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(_scanDelay) };
        _timer.Tick += (_, _) =>
        {
            if (!Device.isOpen)
            {
                return;
            }

            try
            {
                InitDevice();
            }
            catch (Exception exception)
            {
                DestroyDevice();
            }
        };
        _timer.Start();
    }

    private void StopScanDevice()
    {
        if (_timer == null)
        {
            return;
        }

        _timer.Stop();
        _timer = null;
    }

    private void PullData(object _, UsbReportEventArgs args)
    {
        // _reportsCount++;
        Data = StructUtil.ByteToStruct<ReadableDataStructure>(args.Data);
    }

    private static string GetUsbConnectionStatus(UsbConnectionStatusCode status)
    {
        switch (status)
        {
            case UsbConnectionStatusCode.Connected:
                return "Устройство подключено";
            case UsbConnectionStatusCode.Connecting:
                return "Идёт подключение...";
            case UsbConnectionStatusCode.NoConnection:
                return "Нет подключенного устройства";
            default:
                return "";
        }
    }
}