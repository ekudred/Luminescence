using Luminescence.Usb;

namespace Luminescence.Services;

public class ExpDeviceOptions : HidDeviceOptions
{
    public readonly ushort VendorId = 0x0483;
    public readonly ushort ProductId = 0x5750;
    public readonly string SerialNumber = null;
    public readonly int ReadReportLength = 64;
    public readonly int ReadInterval = 10;
    public readonly int CheckInterval = 100;
}