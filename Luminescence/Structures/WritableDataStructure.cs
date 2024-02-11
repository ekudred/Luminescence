using System.Runtime.InteropServices;

namespace Luminescence.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
public struct WritableDataStructure
{
    /** 00     репорт приема команд и данных от ПК, номер типа репорта repStdOutPC = 1 */
    public uint ID_Report;

    /** 01     команда */
    public uint Command;

    /** 02     параметр 0 */
    public uint Parameter0;

    /** 03     параметр 1 */
    public uint Parameter1;


    /** 04     режим нагрева (0 – отключение, 1 – линейный нагрев, 2 – поддержание) */
    public uint HeaterMode;

    /** 05     режим работы светодиодов для ОСЛ (0 – отключение, 1 – линейное увеличение тока, 2 – поддержание тока) */
    public uint LEDMode;

    /** 06     режим работы ФЭУ (0 – ФЭУ выключен, 1 – авто режим, 2 – постоянное Ufeu) */
    public uint PEMMode;

    /** 07     скорость нагрева (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1C°/с, 10 - 1C°/с, 20 - 2C°/с ... ) */
    public uint HeatRate;


    /** 08-09  начальная температура, С° */
    public uint StartTemperature;

    /** 10-11  конечная температура, С° */
    public uint EndTemperature;


    /** 12-13  начальный ток, мА */
    public uint StartLEDCurrent;

    /** 14-15  конечный ток, мА */
    public uint EndLEDCurrent;


    /** 16-17  скорость роста тока (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... ) */
    public uint LEDCurrentRate;

    /** 18     управляющее напряжение на ФЭУ (фикс точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В) коэф. */
    public uint Upem;


    /** 19-62  Data[LengthDataInRepStdOutPC] LengthDataInRepStdOutPC = 43 */
    public uint Data;

    /** 63     Ошибка */
    public uint fError;
}