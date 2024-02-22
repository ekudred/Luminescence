using System;
using System.Threading;
using System.Threading.Tasks;
using Luminescence.Dialog;
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

    public void ConnectDevice()
    {
        StopScanDevice();

        Device = new UsbDevice(0x0483, 0x5750, null, true, 64);
        Device.StartAsyncRead();
    }

    public void DisconnectDevice()
    {
        StopScanDevice();

        Device.StopAsyncRead();
        Device = null;
    }

    public void StartScanDevice()
    {
        if (Device != null && !Device.isOpen)
        {
            return;
        }

        WritableDataStructure b = new WritableDataStructure();
        b.ID_Report = 1;
        b.Command = 1;
        PushData(b);

        Device.InputReportArrivedEvent += PullData;
        Active = true;

        // Task
        //     .Run(() =>
        //     {
        //         Device.InputReportArrivedEvent += PullData;
        //         Device.StartAsyncRead();
        //         Active = true;
        //     })
        //     .Wait(TimeSpan.FromSeconds(2000));
    }

    public void StopScanDevice()
    {
        if (Device == null || !Device.isOpen)
        {
            return;
        }

        WritableDataStructure b = new WritableDataStructure();
        b.ID_Report = 1;
        b.Command = 0x20;
        PushData(b);

        // Device.StopAsyncRead();
        Active = false;

        // Task
        //     .Run(() =>
        //     {
        //         Device.StopAsyncRead();
        //         Active = false;
        //     })
        //     .Wait(TimeSpan.FromSeconds(2000));
    }

    public void PushData(WritableDataStructure data)
    {
        try
        {
            Device.Write(StructUtil.StructToByte(data));
        }
        catch (Exception exception)
        {
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