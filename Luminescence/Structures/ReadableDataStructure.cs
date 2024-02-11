using System.Runtime.InteropServices;

namespace Luminescence.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
public struct ReadableDataStructure
{
    /** 00     стандартный репорт передачи данных от МК, номер типа репорта tReportStdInPC = 3 */
    public uint ID_Report;

    /** 01     режим */
    public uint Mode;

    /** 02     фаза режима */
    public uint Phase;

    /** 03     параметр 0 */
    public uint Parameter0;


    /** 04-07  кол-во тиков после запуска нагрева */
    public uint Counter;


    /** 08     режим нагрева */
    public uint HeaterMode;

    /** 09     скорость нагрева, C°/с */
    public uint HeatingRate;

    /** 10-11  скорость увеличения тока, 0,1мА/с (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... ) */
    public uint LEDCurrentRate;


    /** 12-15  опорная температура */
    public float OpTemperature;

    /** 16-19  текущая температура */
    public float Temperature;

    /** 20-23  текущый ток, мА */
    public float LEDCurrent;


    /** 24-27  интенсивность свечения */
    public uint Intensity;

    /** 28     режим работы светодиодов для ОСЛ */
    public uint LEDMode;

    /** 29     управляющее напряжение на ФЭУ (фикс точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В) */
    public uint Upem;


    /** 30-62  Data[LengthDataInRepStdOutPC] LengthDataInRepStdOutPC = 33 */
    public uint Data;

    /** 63     Ошибка */
    public uint fError;
}