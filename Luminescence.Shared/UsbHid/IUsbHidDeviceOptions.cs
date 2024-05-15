namespace Luminescence.Shared.UsbHid;

public interface IUsbHidDeviceOptions
{
    public ushort VendorId { get; }
    public ushort ProductId { get; }
    public string? SerialNumber { get; }
    public int ReadReportLength { get; }
    public int ReadInterval { get; }
    public int CheckInterval { get; }
}