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