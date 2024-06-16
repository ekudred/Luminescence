using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicData;
using Luminescence.Form;
using Luminescence.Services;
using Luminescence.Shared.Utils;

namespace Luminescence.Models;

public class MeasurementSettingsFormModel : FormModel
{
    public static int GetHeaterMode(string mode)
    {
        switch (mode)
        {
            case MeasurementSettingsFormControl.HeaterOff:
                return 0;
            case MeasurementSettingsFormControl.LinearHeating:
                return 1;
            case MeasurementSettingsFormControl.TemperatureMaintenance:
                return 2;
            default:
                return 0;
        }
    }

    public static int GetLEDMode(string mode)
    {
        switch (mode)
        {
            case MeasurementSettingsFormControl.LedOff:
                return 0;
            case MeasurementSettingsFormControl.LinearIncreaseCurrent:
                return 1;
            case MeasurementSettingsFormControl.CurrentMaintenance:
                return 2;
            default:
                return 0;
        }
    }

    public static int GetPEMMode(string mode)
    {
        switch (mode)
        {
            case MeasurementSettingsFormControl.Automatic:
                return 1;
            case MeasurementSettingsFormControl.Upem:
                return 2;
            default:
                return 1;
        }
    }

    /// Режим
    public int HeaterMode;

    /// Режим светодиода
    public int LEDMode;

    /// Режим ФЭУ
    public int PEMMode;

    /// Конечная температура, °C
    public double EndTemperature;

    /// Скорость нагрева, °C/сек
    public double HeatingRate;

    /// Начальный ток, мА
    public double StartLEDCurrent;

    /// Конечный ток, мА
    public double EndLEDCurrent;

    /// Скорость роста тока, мА/сек
    public double LEDCurrentRate;

    /// Напряжение на ФЭУ, В
    public double Ufeu;

    /// Смещение нуля ЦАП
    public double LedCAPZeroOffset;

    /// Коэффициент преобразования ЦАП
    public double LedCAPCoefTransform;

    /// Изменение кода ФЭУ
    public double CodeChange;

    /// Изменение температуры
    public double TemperatureChange;

    /// Смещение нуля АЦП
    public double ThermocoupleACPZeroOffset;

    /// Коэффициент преобразования АЦП
    public double ThermocoupleACPCoefTransform;

    /// Темновой ток Codes
    public Dictionary<int, double> DarkCurrentCodes;

    /// Коэффициенты чувствительности Coefs
    public Dictionary<int, double> SensitivityCoefs;

    /// Очищение графиков перед измерением
    public bool Clear;

    public override bool Equals(FormModel? obj)
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

            if (value.GetType().GetInterfaces().Contains(typeof(IDictionary<int, double>)))
            {
                equally = ((IDictionary<int, double>)value).SequenceEqual((IDictionary<int, double>)objValue);

                continue;
            }

            equally = value.Equals(objValue);
        }

        return equally;
    }

    public ExpWriteDto ToDto()
    {
        ExpWriteDto dto = new();
        dto.ID_Report = 1;
        dto.Command = 1;
        dto.Parameter0 = 0;
        dto.Parameter1 = 0;
        dto.HeaterMode = Convert.ToByte(HeaterMode);
        dto.LEDMode = Convert.ToByte(LEDMode);
        dto.PEMMode = Convert.ToByte(PEMMode);
        dto.HeatingRate = Convert.ToByte(HeatingRate * 10);
        dto.TemperatureError = 0;
        dto.LEDCurrentRate = Convert.ToUInt16(LEDCurrentRate * 10);
        dto.StartTemperature = 0;
        dto.EndTemperature = Convert.ToUInt16(EndTemperature);
        dto.StartLEDCurrent = Convert.ToUInt16(StartLEDCurrent);
        dto.EndLEDCurrent = Convert.ToUInt16(EndLEDCurrent);
        dto.Upem = Convert.ToByte(Ufeu * 100);
        dto.KeyControl = 0;
        dto.PEMError = 0;
        dto.OffsetADCThermocouple = 0;
        dto.OffsetDACLED = 0;
        dto.CoefADCTemperature = 0;
        dto.CoefDACLED = 0;
        dto.Data = 0;
        dto.fError = 0;

        return dto;
    }

    public override string ToString()
    {
        var a = GetType().GetFields()
            .Select(info =>
            {
                if (
                    info.GetValue(this) != null &&
                    info.GetValue(this)!.GetType().GetInterfaces().Contains(typeof(IDictionary<int, double>))
                )
                {
                    var value = "";
                    var dictionary = (Dictionary<int, double>)info.GetValue(this)!;

                    foreach (var item in dictionary)
                    {
                        var end = dictionary.IndexOf(item) == dictionary.Count ? "" : ".";
                        value += $"{item.Key}:{item.Value}{end}";
                    }

                    return (info.Name, Value: value);
                }

                return (info.Name, Value: info.GetValue(this) ?? "(null)");
            })
            .Aggregate(
                new StringBuilder(),
                (acc, pair) => acc.AppendLine($"{pair.Name};{pair.Value}"),
                acc => acc.ToString()
            );

        return a;
    }
}