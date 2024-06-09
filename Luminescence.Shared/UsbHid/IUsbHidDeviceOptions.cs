namespace Luminescence.Shared.UsbHid;

public interface IUsbHidDeviceOptions
{
    /** Иидентификатор производителя устройства */
    public ushort VendorId { get; }
    /** Идентификатор устройства */
    public ushort ProductId { get; }
    /** Серийный номер устройства */
    public string? SerialNumber { get; }
    /** Длина отчетов, которые отправляет устройство */
    public int ReadReportLength { get; }
    /** Интервал времени в миллисекундах, через который отправляется команда чтения */
    public int ReadInterval { get; }
    /** Интервал времени в миллисекундах, через который идет проверка на подключение к устройству */
    public int CheckInterval { get; }
}