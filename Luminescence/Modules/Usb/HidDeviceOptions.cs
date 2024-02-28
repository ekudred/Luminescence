namespace Luminescence.Usb;

public class HidDeviceOptions
{
    public readonly ushort VendorId;
    public readonly ushort ProductId;
    public readonly string? SerialNumber = null;
    public readonly int ReadReportLength = 64;
    public readonly int ReadInterval = 10;
    public readonly int CheckInterval = 100;
}