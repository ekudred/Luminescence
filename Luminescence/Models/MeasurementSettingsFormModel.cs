using System;
using System.Collections.Generic;
using System.Linq;
using Luminescence.Form;

namespace Luminescence.Models;

public class MeasurementSettingsFormModel : FormBaseModel
{
    /** Нагреватель выключен */
    public bool HeaterOff;

    /** Линейный нагрев */
    public bool LinearHeating;

    /** Поддержание температуры */
    public bool TemperatureMaintenance;

    /** Конечная температура, °C */
    public int EndTemperature;

    /** Скорость нагрева, °C/сек */
    public int HeatingRate;

    /** Светодиод выключен */
    public bool LedOff;

    /** Линейное увеличение тока */
    public bool LinearIncreaseCurrent;

    /** Поддержание тока */
    public bool CurrentMaintenance;

    /** Начальный ток, мА */
    public int StartLEDCurrent;

    /** Конечный ток, мА */
    public int EndLEDCurrent;

    /** Скорость роста тока, мА/сек */
    public int LEDCurrentRate;

    /** Автоматический */
    public bool Automatic;

    /** Управляющее напряжение, В */
    public bool Upem;

    /** Напряжение на ФЭУ, В */
    public int Ufeu;

    /** Смещение нуля ЦАП */
    public int LedCAPZeroOffset;

    /** Коэффициент преобразования ЦАП */
    public int LedCAPCoefTransform;

    /** Изменение кода ФЭУ */
    public int CodeChange;

    /** Изменение температуры */
    public int TemperatureChange;

    /** Смещение нуля АЦП */
    public int ThermocoupleACPZeroOffset;

    /** Коэффициент преобразования АЦП */
    public int ThermocoupleACPCoefTransform;

    /** Темновой ток Codes */
    public Dictionary<string, string> DarkCurrentCodes;

    /** Коэффициенты чувствительности Coefs */
    public Dictionary<string, string> SensitivityCoefs;

    public byte GetHeaterMode()
    {
        if (LedOff)
        {
            return 0;
        }

        if (LinearHeating)
        {
            return 1;
        }

        if (TemperatureMaintenance)
        {
            return 2;
        }

        return 0;
    }

    public byte GetLEDMode()
    {
        if (HeaterOff)
        {
            return 0;
        }

        if (LinearIncreaseCurrent)
        {
            return 1;
        }

        if (CurrentMaintenance)
        {
            return 2;
        }

        return 0;
    }

    public byte GetPEMMode()
    {
        if (Automatic)
        {
            return 1;
        }

        if (Upem)
        {
            return 2;
        }

        return 1;
    }

    public override bool Equals(FormBaseModel? obj)
    {
        if (obj == null)
        {
            return false;
        }

        var equally = true;

        foreach (var field in GetType().GetFields())
        {
            if (!equally)
            {
                break;
            }

            var value = field.GetValue(this);
            var objValue = obj.GetType().GetField(field.Name)!.GetValue(obj);

            if (value == null || objValue == null)
            {
                equally = value == objValue;

                continue;
            }

            if (value.GetType().GetInterfaces().Contains(typeof(IDictionary<string, string>)))
            {
                equally = ((IDictionary<string, string>)value).SequenceEqual((IDictionary<string, string>)objValue);

                continue;
            }

            equally = value.Equals(objValue);
        }

        return equally;
    }
}