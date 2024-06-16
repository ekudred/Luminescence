using System;
using System.Runtime.InteropServices;
using Luminescence.Shared.Utils;

namespace Luminescence.Services;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
public struct ExpWriteDto
{
    public static ExpWriteDto StopDto => new() { ID_Report = 1, Command = 0x20 };

    /// 00     Репорт приема команд и данных от ПК, номер типа репорта repStdOutPC = 1
    public byte ID_Report;

    /// 01     Команда
    public byte Command;

    /// 02     Параметр 0
    public byte Parameter0;

    /// 03     Параметр 1
    public byte Parameter1;

    /// 04     Режим нагрева (0 – отключение, 1 – линейный нагрев, 2 – поддержание)
    public byte HeaterMode;

    /// 05     Режим работы светодиодов для ОСЛ (0 – отключение, 1 – линейное увеличение тока, 2 – поддержание тока)
    public byte LEDMode;

    /// 06     Режим работы ФЭУ (0 – ФЭУ выключен, 1 – авто режим, 2 – постоянное Ufeu)
    public byte PEMMode;

    /// 07     Скорость нагрева (фикс. точка 1 знак после запятой (входное число делим на 10): 1 - 0,1C°/с, 10 - 1C°/с, 20 - 2C°/с ... )
    public byte HeatingRate;

    /// 08-09  Ошибка температуры
    public UInt16 TemperatureError;

    /// 10-11  Скорость роста тока (фикс. точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... )
    public UInt16 LEDCurrentRate;

    /// 12-13  Начальная температура, С°
    public UInt16 StartTemperature;

    /// 14-15  Конечная температура, С°
    public UInt16 EndTemperature;

    /// 16-17  Начальный ток, мА
    public UInt16 StartLEDCurrent;

    /// 18-19  Конечный ток, мА
    public UInt16 EndLEDCurrent;

    /// 20     Управляющее напряжение на ФЭУ (фикс. точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В) коэф.
    public byte Upem;

    /// 21     Управление состояниями ключей
    public byte KeyControl;

    /// 22-23  Ошибка PEM
    public UInt16 PEMError;

    /// 24-25  Смещение нуля АЦП термопары
    public UInt16 OffsetADCThermocouple;

    /// 26-27  Смещение нуля ЦАП светодиодов
    public UInt16 OffsetDACLED;

    /// 28-31  Коэффициент преобразования АЦП термопары
    public float CoefADCTemperature;

    /// 32-35  Коэффициент преобразования ЦАП светодиодов
    public float CoefDACLED;

    /// 36-62  Data
    public byte Data;

    /// 63     Ошибка
    public uint fError;

    public ExpWriteDto GetRunDto()
    {
        ID_Report = 1;
        Command = 1;

        return this;
    }

    public byte[] ToBytes()
    {
        return StructUtil.StructToBytes(this);
    }
}