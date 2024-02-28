using System;

namespace Luminescence.Usb;

public class HidReportEventArgs : EventArgs
{
    public readonly byte[] Data;

    public HidReportEventArgs(byte[] data)
    {
        Data = data;
    }
}