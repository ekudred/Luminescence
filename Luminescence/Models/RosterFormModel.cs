using Luminescence.Form;

namespace Luminescence.Models;

public class RosterFormModel : FormBaseModel
{
    /** Нагреватель выключен */
    public bool HeaterOff;
    /** Линейный нагрев */
    public bool LinearHeating;
    /** Поддержание температуры */
    public bool TemperatureMaintenance;
    /** Конечная температура, °C */
    public string EndTemperature;
    /** Скорость нагрева, °C/сек */
    public string HeatRate;

    /** Светодиод выключен */
    public bool LedOff;
    /** Линейное увеличение тока */
    public bool LinearIncreaseCurrent;
    /** Поддержание тока */
    public bool CurrentMaintenance;
    /** Начальный ток, мА */
    public string StartLEDCurrent;
    /** Конечный ток, мА */
    public string EndLEDCurrent;
    /** Скорость роста тока, мА/сек */
    public string LEDCurrentRate;

    /** Автоматический */
    public bool Automatic;
    /** Управляющее напряжение, В */
    public bool Upem;
    /** * Label */
    public string Label;
}