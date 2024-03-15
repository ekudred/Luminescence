using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Luminescence.Form.ViewModels;
using Luminescence.Models;
using Luminescence.Utils;

namespace Luminescence.ViewModels;

public class MeasurementSettingsFormViewModel : FormViewModel<MeasurementSettingsFormModel>
{
    public List<FormControlBaseViewModel> DarkCurrentCodeControls => _darkCurrentCodeControls;
    public List<FormControlBaseViewModel> SensitivityCoefControls => _sensitivityCoefControls;

    private List<FormControlBaseViewModel> _darkCurrentCodeControls = new();
    private List<FormControlBaseViewModel> _sensitivityCoefControls = new();

    private readonly double _darkCurrentCodesStartValue = 0.50;
    private readonly double _darkCurrentCodesIncrementValue = 0.05;
    private readonly int _darkCurrentCodesCount = 13;
    private readonly double _sensitivityCoefsStartValue = 0.50;
    private readonly double _sensitivityCoefsIncrementValue = 0.05;
    private readonly int _sensitivityCoefsCount = 13;

    public override void FromModel(MeasurementSettingsFormModel model)
    {
        SetControlValue("HeaterMode", GetHeaterMode(model.HeaterMode));
        SetControlValue("EndTemperature", model.EndTemperature.FromObject());
        SetControlValue("HeatingRate", model.HeatingRate.FromObject());

        SetControlValue("LEDMode", GetLEDMode(model.LEDMode));
        SetControlValue("StartLEDCurrent", model.StartLEDCurrent);
        SetControlValue("EndLEDCurrent", model.EndLEDCurrent.FromObject());
        SetControlValue("LEDCurrentRate", model.LEDCurrentRate.FromObject());

        SetControlValue("PEMMode", GetPEMMode(model.PEMMode));
        SetControlValue("Ufeu", model.Ufeu.FromObject());

        SetControlValue("LedCAPZeroOffset", model.LedCAPZeroOffset.FromObject());
        SetControlValue("LedCAPCoefTransform", model.LedCAPCoefTransform.FromObject());
        SetControlValue("CodeChange", model.CodeChange.FromObject());
        SetControlValue("TemperatureChange", model.TemperatureChange.FromObject());
        SetControlValue("ThermocoupleACPZeroOffset", model.ThermocoupleACPZeroOffset.FromObject());
        SetControlValue("ThermocoupleACPCoefTransform", model.ThermocoupleACPCoefTransform.FromObject());

        foreach (var darkCurrentCode in model.DarkCurrentCodes)
        {
            SetControlValue($"DarkCurrentCode{darkCurrentCode.Key}",
                darkCurrentCode.Value.ToString(CultureInfo.InvariantCulture));
        }

        foreach (var sensitivityCoef in model.SensitivityCoefs)
        {
            SetControlValue($"SensitivityCoef{sensitivityCoef.Key}",
                sensitivityCoef.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    protected override void ChangeModel(MeasurementSettingsFormModel model)
    {
        string heaterMode = GetControlValue<string>("HeaterMode")!;
        model.HeaterMode = MeasurementSettingsFormModel.GetHeaterMode(heaterMode);

        model.EndTemperature = GetControlValue<string>("EndTemperature")!.ToDouble() ?? 0;
        model.HeatingRate = GetControlValue<string>("HeatingRate")!.ToDouble() ?? 0;

        string LEDMode = GetControlValue<string>("LEDMode")!;
        model.LEDMode = MeasurementSettingsFormModel.GetLEDMode(LEDMode);

        model.StartLEDCurrent = GetControlValue<decimal>("StartLEDCurrent").ToDouble() ?? 0;
        model.EndLEDCurrent = GetControlValue<string>("EndLEDCurrent")!.ToDouble() ?? 0;
        model.LEDCurrentRate = GetControlValue<string>("LEDCurrentRate")!.ToDouble() ?? 0;

        string PEMMode = GetControlValue<string>("PEMMode")!;
        model.PEMMode = MeasurementSettingsFormModel.GetPEMMode(PEMMode);

        model.Ufeu = GetControlValue<string>("Ufeu")!.ToDouble() ?? 0;

        model.LedCAPZeroOffset = GetControlValue<string>("LedCAPZeroOffset")!.ToDouble() ?? 0;
        model.LedCAPCoefTransform = GetControlValue<string>("LedCAPCoefTransform")!.ToDouble() ?? 0;
        model.CodeChange = GetControlValue<string>("CodeChange")!.ToDouble() ?? 0;
        model.TemperatureChange = GetControlValue<string>("TemperatureChange")!.ToDouble() ?? 0;
        model.ThermocoupleACPZeroOffset = GetControlValue<string>("ThermocoupleACPZeroOffset")!.ToDouble() ?? 0;
        model.ThermocoupleACPCoefTransform = GetControlValue<string>("ThermocoupleACPCoefTransform")!.ToDouble() ?? 0;

        model.DarkCurrentCodes = new();
        _darkCurrentCodeControls.ForEach(control =>
        {
            var key = control.Name.Replace("DarkCurrentCode", string.Empty);
            var value = ((string)control.Value).ToDouble() ?? 0;
            model.DarkCurrentCodes.Add(key, value);
        });

        model.SensitivityCoefs = new();
        _sensitivityCoefControls.ForEach(control =>
        {
            var key = control.Name.Replace("SensitivityCoef", string.Empty);
            var value = ((string)control.Value).ToDouble() ?? 0;
            model.SensitivityCoefs.Add(key, value);
        });
    }

    protected override List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        List<RadioControlViewModel> heaterModeControlGroup = new()
        {
            new("HeaterOff", new() { Label = "Нагреватель выключен" }),
            new("LinearHeating", new() { Label = "Линейный нагрев" }),
            new("TemperatureMaintenance", new() { Label = "Поддержание температуры" })
        };
        list.Add(new RadioGroupControlViewModel("HeaterMode", "HeaterOff", heaterModeControlGroup));

        list.Add(new TextControlViewModel("EndTemperature", "0", new() { Label = "Конечная температура, °C" }));
        list.Add(new TextControlViewModel("HeatingRate", "0", new() { Label = "Скорость нагрева, °C/сек" }));

        List<RadioControlViewModel> LEDModeControlGroup = new()
        {
            new("LedOff", new() { Label = "Светодиод выключен" }),
            new("LinearIncreaseCurrent", new() { Label = "Линейное увеличение тока" }),
            new("CurrentMaintenance", new() { Label = "Поддержание тока" })
        };
        list.Add(new RadioGroupControlViewModel("LEDMode", "LedOff", LEDModeControlGroup));

        list.Add(new NumericControlViewModel("StartLEDCurrent", 0, new() { Label = "Начальный ток, мА" }));
        list.Add(new TextControlViewModel("EndLEDCurrent", "0", new() { Label = "Конечный ток, мА" }));
        list.Add(new TextControlViewModel("LEDCurrentRate", "0", new() { Label = "Скорость роста тока, мА/сек" }));

        List<RadioControlViewModel> PEMModeControlGroup = new()
        {
            new("Automatic", new() { Label = "Автоматический" }),
            new("Upem", new() { Label = "Управляющее напряжение, В" })
        };
        list.Add(new RadioGroupControlViewModel("PEMMode", "Automatic", PEMModeControlGroup));

        list.Add(new TextControlViewModel("Ufeu", "0", new() { Label = "Напряжение на ФЭУ" }));

        list.Add(new TextControlViewModel("LedCAPZeroOffset", "0", new() { Label = "Смещение нуля ЦАП" }));
        list.Add(new TextControlViewModel("LedCAPCoefTransform", "0",
            new() { Label = "Коэффициент преобразования ЦАП" }));
        list.Add(new TextControlViewModel("CodeChange", "0", new() { Label = "Изменение кода ФЭУ" }));
        list.Add(new TextControlViewModel("TemperatureChange", "0", new() { Label = "Изменение температуры" }));
        list.Add(new TextControlViewModel("ThermocoupleACPZeroOffset", "0", new() { Label = "Смещение нуля АЦП" }));
        list.Add(new TextControlViewModel("ThermocoupleACPCoefTransform", "0",
            new() { Label = "Коэффициент преобразования АЦП" }));

        for (int i = 0; i < _darkCurrentCodesCount - 1; i++)
        {
            _darkCurrentCodeControls.Add(new TextControlViewModel($"DarkCurrentCode{i}", "0",
                new() { Label = $"{_darkCurrentCodesStartValue + _darkCurrentCodesIncrementValue * i}", }));
        }

        for (int i = 0; i < _sensitivityCoefsCount - 1; i++)
        {
            _sensitivityCoefControls.Add(new TextControlViewModel($"SensitivityCoef{i}", "0",
                new() { Label = $"{_sensitivityCoefsStartValue + _sensitivityCoefsIncrementValue * i}" }));
        }

        return list
            .Concat(_darkCurrentCodeControls)
            .Concat(_sensitivityCoefControls)
            .ToList();
    }

    private string GetHeaterMode(int mode)
    {
        switch (mode)
        {
            case 0:
                return "HeaterOff";
            case 1:
                return "LinearHeating";
            case 2:
                return "TemperatureMaintenance";
            default:
                return "HeaterOff";
        }
    }

    private string GetLEDMode(int mode)
    {
        switch (mode)
        {
            case 0:
                return "LedOff";
            case 1:
                return "LinearIncreaseCurrent";
            case 2:
                return "CurrentMaintenance";
            default:
                return "LedOff";
        }
    }

    private string GetPEMMode(int mode)
    {
        switch (mode)
        {
            case 1:
                return "Automatic";
            case 2:
                return "Upem";
            default:
                return "Automatic";
        }
    }
}