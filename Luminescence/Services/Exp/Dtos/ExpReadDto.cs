using System;
using System.Runtime.InteropServices;
using Luminescence.Utils;

namespace Luminescence.Services;

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
    public byte Phase;

    /** 03     параметр 0 */
    public byte Parameter0;

    /** 04-07  кол-во тиков после запуска нагрева */
    public UInt32 Counter;

    /** 28-31  интенсивность свечения */
    public UInt32 Intensity;

    /** 08     режим нагрева */
    public byte HeaterMode;

    /** 09     скорость нагрева, C°/с */
    public byte HeatingRate;

    /** 10-11  скорость увеличения тока, 0,1мА/с (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... ) */
    public UInt16 LEDCurrentRate;

    /** 12-15  опорная температура */
    public float OpTemperature;

    /** 16-19  текущая температура */
    public float Temperature;

    /** 20-23  опорный ток, мА */
    public float OpLEDCurrent;

    /** 24-27  текущый ток, мА */
    public float LEDCurrent;

    /** 32     режим работы светодиодов для ОСЛ */
    public byte LEDMode;

    /** 33     управляющее напряжение на ФЭУ (фикс точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В) */
    public byte Upem;

    /** 34     авто напряжение на ФЭУ */
    public byte AutoUpem;

    /** 35-62  */
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
    public byte Data;

    /** 63     Ошибка */
    public byte fError;
}