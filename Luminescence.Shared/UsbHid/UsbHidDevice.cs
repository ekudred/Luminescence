using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;

namespace Luminescence.Shared.UsbHid;

public class UsbHidDevice
{
    public readonly BehaviorSubject<bool> Connected = new(false);

    protected nint DeviceHandle;

    protected readonly Subject<byte[]> ReadData = new();
    protected readonly Subject<Exception> ReadException = new();
    protected readonly Subject<bool> СonnectionLost = new();
    protected readonly UsbHidService UsbHidService;

    private bool _opened => DeviceHandle != IntPtr.Zero;

    private Subject<int> _listenDeviceOn;
    private Subject<int> _checkOn;

    private readonly IUsbHidDeviceOptions _options;

    // test
    public bool TestActive = false;
    public readonly bool TestMode = false;
    // end test

    protected UsbHidDevice(
        IUsbHidDeviceOptions options,
        UsbHidService usbUsbHidService
    )
    {
        _options = options;
        UsbHidService = usbUsbHidService;
    }

    public void RunAvailableDeviceCheck()
    {
        if (TestMode)
        {
            Connect();

            return;
        }

        // end test
        if (_checkOn != null)
        {
            return;
        }

        _checkOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.CheckInterval))
            .Select(_ => UsbHidService.Enumerate(_options.VendorId, _options.ProductId)).Switch()
            .TakeUntil(_checkOn)
            .Subscribe(
                data =>
                {
                    if (!_opened && data != IntPtr.Zero)
                    {
                        Connect();
                    }

                    if (_opened && data == IntPtr.Zero)
                    {
                        СonnectionLost.OnNext(true);
                        Disconnect();
                    }
                }
            );
    }

    public void StopAvailableDeviceCheck()
    {
        if (_checkOn == null)
        {
            return;
        }

        _checkOn.OnNext(0);
        _checkOn.OnCompleted();
        _checkOn = null;
    }

    public void Connect()
    {
        ConnectDevice();
        StartListenDevice();
    }

    public void Disconnect()
    {
        DisconnectDevice();
        StopListenDevice();
    }

    private void ConnectDevice()
    {
        if (TestMode)
        {
            Connected.OnNext(true);

            return;
        }

        // end test
        if (_opened)
        {
            return;
        }

        UsbHidService.Open(_options.VendorId, _options.ProductId, _options.SerialNumber)
            .Subscribe(
                deviceHandle =>
                {
                    DeviceHandle = deviceHandle;
                    Connected.OnNext(_opened);
                },
                () => { }
            );
    }

    private void DisconnectDevice()
    {
        if (TestMode)
        {
            return;
        }
        // end test

        if (!_opened)
        {
            return;
        }

        UsbHidService.Close(DeviceHandle)
            .Subscribe();

        DeviceHandle = IntPtr.Zero;
        Connected.OnNext(_opened);
    }

    private void StartListenDevice()
    {
        if (TestMode)
        {
            if (_listenDeviceOn != null)
            {
                return;
            }

            _listenDeviceOn = new();

            Random random = new Random();
            double counter = 0;

            Observable
                .Interval(TimeSpan.FromMilliseconds(_options.ReadInterval))
                .Where(_ => TestActive)
                .TakeUntil(_listenDeviceOn)
                .Subscribe(_ =>
                {
                    var temperature = Math.Truncate(random.NextSingle() * 1000);
                    var intensity = Math.Truncate(random.NextDouble() * 500);
                    var LEDCurrent = Math.Truncate(random.NextDouble());

                    var structure = new ExpReadDto();
                    structure.Counter += UInt32.Parse(counter.ToString());
                    structure.Temperature = float.Parse(temperature.ToString());
                    structure.Intensity = UInt32.Parse(intensity.ToString());
                    structure.LEDCurrent = UInt32.Parse(LEDCurrent.ToString());

                    ReadData.OnNext(StructUtil.StructToBytes(structure));

                    counter += 1;
                });

            return;
        }
        // end test

        if (!_opened || _listenDeviceOn != null)
        {
            return;
        }

        _listenDeviceOn = new();

        Observable
            .Interval(TimeSpan.FromMilliseconds(_options.ReadInterval))
            .Select(_ => UsbHidService.Read(DeviceHandle, _options.ReadReportLength)).Switch()
            .TakeUntil(_listenDeviceOn)
            .Subscribe(
                data => { ReadData.OnNext(data); },
                exception => { ReadException.OnNext(exception); }
            );
    }

    private void StopListenDevice()
    {
        if (_listenDeviceOn == null)
        {
            return;
        }

        _listenDeviceOn.OnNext(0);
        _listenDeviceOn.OnCompleted();
        _listenDeviceOn = null;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
public struct ExpReadDto
{
    public static ExpReadDto FromBytes(byte[] data)
    {
        return StructUtil.BytesToStruct<ExpReadDto>(data);
    }

    /** 00     стандартный репорт передачи данных от МК, номер типа репорта tReportStdInPC = 3 */
    public byte ID_Report;

    /** 01     режим */
    public byte Mode;

    /** 02     фаза режима */
    public byte Phase; // -

    /** 03     параметр 0 */
    public byte Parameter0;

    /** 04-07  кол-во тиков после запуска нагрева */
    public UInt32 Counter; // время 100 имс/c

    /** 28-31  интенсивность свечения */
    public UInt32 Intensity; // Графики + код ацп фэу

    /** 08     режим нагрева */
    public byte HeaterMode; // Индикатор что нагреватель включен 0 - не вкл, другое - включен

    /** 09     скорость нагрева, C°/с */
    public byte HeatingRate; // - отправил и возвращается нам

    /** 10-11  скорость увеличения тока, 0,1мА/с (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... ) */
    public UInt16 LEDCurrentRate; //  - отправил и возвращается нам

    /** 12-15  опорная температура */
    public float OpTemperature; // Графики 1 3

    /** 16-19  текущая температура */
    public float Temperature; // Графики 1 + индикатор

    /** 20-23  опорный ток, мА */
    public float OpLEDCurrent; // -

    /** 24-27  текущый ток, мА */
    public float LEDCurrent; // + Ток индикатор

    /** 32     режим работы светодиодов для ОСЛ */
    public byte LEDMode; // 0 - светодиод выключен, другое включен

    /** 33     управляющее напряжение на ФЭУ (фикс точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В) */
    public byte Upem; // + Ufeu/100 индикатор

    /** 34     авто напряжение на ФЭУ */
    public byte AutoUpem; // - Авторежим в 3 вкладке

    /** 35-62  */
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
    public byte Data;

    /** 63     Ошибка */
    public byte fError; // если 0 - ошибки, 1 - ошибка нагревателя, 2 - ошибка измерений температуры и тд
}

public static class StructUtil
{
    public static T BytesToStruct<T>(byte[] data)
        where T : struct
    {
        var pData = GCHandle.Alloc(data, GCHandleType.Pinned);
        T result = (T)Marshal.PtrToStructure(pData.AddrOfPinnedObject(), typeof(T))!;
        pData.Free();

        return result;
    }

    public static byte[] StructToBytes<T>(T data)
        where T : struct
    {
        byte[] result = new byte[Marshal.SizeOf(typeof(T))];
        var pResult = GCHandle.Alloc(result, GCHandleType.Pinned);
        Marshal.StructureToPtr(data, pResult.AddrOfPinnedObject(), true);
        pResult.Free();

        return result;
    }
}