using System;
using Avalonia.Threading;
using Luminescence.Dialog;
using Luminescence.Usb;
using Luminescence.Enums;
using Luminescence.Models;
using Luminescence.Utils;
using Luminescence.Views;
using ReactiveUI;
using Tmds.DBus;

namespace Luminescence.Services;

public class ExpUsbDeviceService : ReactiveObject
{
    public UsbDevice? Device
    {
        get => _device;
        private set
        {
            if (value == null)
            {
                _device.Dispose();
            }

            this.RaiseAndSetIfChanged(ref _device, value);

            ConnectionStatusCode = Device != null && Device.isOpen
                ? UsbConnectionStatusCode.Connected
                : UsbConnectionStatusCode.NoConnection;
        }
    }

    public bool Active
    {
        get => _active;
        private set => this.RaiseAndSetIfChanged(ref _active, value);
    }

    public ReadableDataStructure Data
    {
        get => _data;
        private set => this.RaiseAndSetIfChanged(ref _data, value);
    }

    public UsbConnectionStatusCode ConnectionStatusCode
    {
        get => _connectionStatusCode;
        private set => this.RaiseAndSetIfChanged(ref _connectionStatusCode, value);
    }

    private UsbDevice _device;

    private bool _active = false;

    private ReadableDataStructure _data;

    private UsbConnectionStatusCode _connectionStatusCode = UsbConnectionStatusCode.NoConnection;

    // private DispatcherTimer _timer;

    // private readonly int _scanDelay = 1;

    private readonly DialogService _dialogService;

    public ExpUsbDeviceService(DialogService dialogService)
    {
        _dialogService = dialogService;
    }

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

    public void ConnectDevice()
    {
        StopScanDevice();

        Device = new UsbDevice(0x0483, 0x5750, null, true, 64);
    }

    public void DisconnectDevice()
    {
        StopScanDevice();

        Device = null;
    }

    public void StartScanDevice()
    {
        if (Device != null && !Device.isOpen)
        {
            return;
        }

        try
        {
            Device.InputReportArrivedEvent += PullData;
            Device.StartAsyncRead();
            Active = true;
        }
        catch (Exception exception)
        {
            // _dialogService.ShowDialog(new FailDialog());
        }
    }

    public void StopScanDevice()
    {
        if (Device == null || !Device.isOpen)
        {
            return;
        }

        try
        {
            Device.StopAsyncRead();
            Active = false;
        }
        catch (Exception exception)
        {
            // _dialogService.ShowDialog(new FailDialog());
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
            // _dialogService.ShowDialog(new FailDialog());
        }
    }

    // private void StartScanDevice()
    // {
    //     _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(_scanDelay) };
    //     _timer.Tick += (_, _) =>
    //     {
    //         if (!Device.isOpen)
    //         {
    //             return;
    //         }
    //
    //         try
    //         {
    //             InitDevice();
    //         }
    //         catch (Exception exception)
    //         {
    //             DestroyDevice();
    //             _dialogService.ShowDialog(new FailDialog());
    //         }
    //     };
    //     _timer.Start();
    // }
    //
    // private void StopScanDevice()
    // {
    //     if (_timer == null)
    //     {
    //         return;
    //     }
    //
    //     _timer.Stop();
    //     _timer = null;
    // }

    private void PullData(object _, UsbReportEventArgs args)
    {
        // _reportsCount++;
        Data = StructUtil.ByteToStruct<ReadableDataStructure>(args.Data);
    }
}