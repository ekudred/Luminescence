using System;

namespace Luminescence.Usb;

public class UsbReportEventArgs : EventArgs
{
    public UsbReportEventArgs(byte[] data)
    {
        Data = data;
    }

    public byte[] Data { get; }
}