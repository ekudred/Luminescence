using System;
using System.Runtime.InteropServices;
using Luminescence.Shared.Utils;

namespace Luminescence.Services;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
public struct ExpReadDto
{
    public static ExpReadDto FromBytes(byte[] data)
    {
        return StructUtil.BytesToStruct<ExpReadDto>(data);
    }

    /// 00     Стандартный репорт передачи данных от МК, номер типа репорта tReportStdInPC = 3
    public byte ID_Report;

    /// 01     Режим
    public byte Mode;

    /// 02     Фаза режима
    public byte Phase; // -

    /// 03     Параметр 0
    public byte Parameter0;

    /// 04-07  Количество тиков после запуска нагрева
    public UInt32 Counter;

    /// 28-31  Интенсивность свечения
    public UInt32 Intensity;

    /// 08     Режим нагрева
    public byte HeaterMode;

    /// 09     Скорость нагрева, C°/с
    public byte HeatingRate;

    /// 10-11  Скорость увеличения тока, 0,1мА/с (фиксю точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... )
    public UInt16 LEDCurrentRate;

    /// 12-15  Опорная температура
    public float OpTemperature;

    /// 16-19  Текущая температура
    public float Temperature;

    /// 20-23  Опорный ток, мА
    public float OpLEDCurrent;

    /// 24-27  Текущый ток, мА
    public float LEDCurrent;

    /// 32     Режим работы светодиодов для ОСЛ
    public byte LEDMode;

    /// 33     Управляющее напряжение на ФЭУ (фиксю точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В)
    public byte Upem;

    /// 34     Авто напряжение на ФЭУ
    public byte AutoUpem;

    /// 35-62  Data
    public byte Data;

    /// 63     Ошибка
    public byte fError;
}