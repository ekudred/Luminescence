using System;

namespace Luminescence.Usb;

public class HidReadErrorEventArgs : EventArgs
{
    public readonly Exception Error;

    public HidReadErrorEventArgs(Exception error)
    {
        Error = error;
    }
}