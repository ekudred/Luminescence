using Luminescence.Usb;

namespace Luminescence.Services;

public class ExpDeviceOptions : IHidDeviceOptions
{
    public ushort VendorId { get; }
    public ushort ProductId { get; }
    public string? SerialNumber { get; }
    public int ReadReportLength { get; }
    public int ReadInterval { get; }
    public int CheckInterval { get; }

    public ExpDeviceOptions()
    {
        VendorId = 0x0483;
        ProductId = 0x5750;
        SerialNumber = null;
        ReadReportLength = 64;
        ReadInterval = 100;
        CheckInterval = 200;
    }
}