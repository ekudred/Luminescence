using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Luminescence.Form.ViewModels;
using Luminescence.Models;
using Luminescence.Services;
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
        SetControlValue(MeasurementSettingsFormControl.HeaterMode, GetHeaterMode(model.HeaterMode));
        SetControlValue(MeasurementSettingsFormControl.EndTemperature, model.EndTemperature.FromObject());
        SetControlValue(MeasurementSettingsFormControl.HeatingRate, model.HeatingRate.FromObject());

        SetControlValue(MeasurementSettingsFormControl.LEDMode, GetLEDMode(model.LEDMode));
        SetControlValue(MeasurementSettingsFormControl.StartLEDCurrent, model.StartLEDCurrent);
        SetControlValue(MeasurementSettingsFormControl.EndLEDCurrent, model.EndLEDCurrent.FromObject());
        SetControlValue(MeasurementSettingsFormControl.LEDCurrentRate, model.LEDCurrentRate.FromObject());

        SetControlValue(MeasurementSettingsFormControl.PEMMode, GetPEMMode(model.PEMMode));
        SetControlValue(MeasurementSettingsFormControl.Ufeu, model.Ufeu.FromObject());

        SetControlValue(MeasurementSettingsFormControl.LedCAPZeroOffset, model.LedCAPZeroOffset.FromObject());
        SetControlValue(MeasurementSettingsFormControl.LedCAPCoefTransform, model.LedCAPCoefTransform.FromObject());
        SetControlValue(MeasurementSettingsFormControl.CodeChange, model.CodeChange.FromObject());
        SetControlValue(MeasurementSettingsFormControl.TemperatureChange, model.TemperatureChange.FromObject());
        SetControlValue(MeasurementSettingsFormControl.ThermocoupleACPZeroOffset,
            model.ThermocoupleACPZeroOffset.FromObject());
        SetControlValue(MeasurementSettingsFormControl.ThermocoupleACPCoefTransform,
            model.ThermocoupleACPCoefTransform.FromObject());

        foreach (var darkCurrentCode in model.DarkCurrentCodes)
        {
            SetControlValue(MeasurementSettingsFormControl.DarkCurrentCodeName(darkCurrentCode.Key),
                darkCurrentCode.Value.ToString(CultureInfo.InvariantCulture));
        }

        foreach (var sensitivityCoef in model.SensitivityCoefs)
        {
            SetControlValue(MeasurementSettingsFormControl.SensitivityCoefName(sensitivityCoef.Key),
                sensitivityCoef.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    protected override void ChangeModel(MeasurementSettingsFormModel model)
    {
        model.HeaterMode =
            MeasurementSettingsFormModel.GetHeaterMode(
                GetControlValue<string>(MeasurementSettingsFormControl.HeaterMode)!);
        model.EndTemperature = GetControlValue<string>(MeasurementSettingsFormControl.EndTemperature)!.ToDouble() ?? 0;
        model.HeatingRate = GetControlValue<string>(MeasurementSettingsFormControl.HeatingRate)!.ToDouble() ?? 0;

        model.LEDMode =
            MeasurementSettingsFormModel.GetLEDMode(GetControlValue<string>(MeasurementSettingsFormControl.LEDMode)!);

        model.StartLEDCurrent =
            GetControlValue<decimal>(MeasurementSettingsFormControl.StartLEDCurrent).ToDouble() ?? 0;
        model.EndLEDCurrent = GetControlValue<string>(MeasurementSettingsFormControl.EndLEDCurrent)!.ToDouble() ?? 0;
        model.LEDCurrentRate = GetControlValue<string>(MeasurementSettingsFormControl.LEDCurrentRate)!.ToDouble() ?? 0;

        model.PEMMode =
            MeasurementSettingsFormModel.GetPEMMode(GetControlValue<string>(MeasurementSettingsFormControl.PEMMode)!);

        model.Ufeu = GetControlValue<string>(MeasurementSettingsFormControl.Ufeu)!.ToDouble() ?? 0;

        model.LedCAPZeroOffset =
            GetControlValue<string>(MeasurementSettingsFormControl.LedCAPZeroOffset)!.ToDouble() ?? 0;
        model.LedCAPCoefTransform =
            GetControlValue<string>(MeasurementSettingsFormControl.LedCAPCoefTransform)!.ToDouble() ?? 0;
        model.CodeChange = GetControlValue<string>(MeasurementSettingsFormControl.CodeChange)!.ToDouble() ?? 0;
        model.TemperatureChange =
            GetControlValue<string>(MeasurementSettingsFormControl.TemperatureChange)!.ToDouble() ?? 0;
        model.ThermocoupleACPZeroOffset =
            GetControlValue<string>(MeasurementSettingsFormControl.ThermocoupleACPZeroOffset)!.ToDouble() ?? 0;
        model.ThermocoupleACPCoefTransform =
            GetControlValue<string>(MeasurementSettingsFormControl.ThermocoupleACPCoefTransform)!.ToDouble() ?? 0;

        model.DarkCurrentCodes = new();
        _darkCurrentCodeControls.ForEach(control =>
        {
            var key = MeasurementSettingsFormControl.DarkCurrentCodeKey(control.Name);
            var value = ((string)control.Value).ToDouble() ?? 0;
            model.DarkCurrentCodes.Add(key, value);
        });

        model.SensitivityCoefs = new();
        _sensitivityCoefControls.ForEach(control =>
        {
            var key = MeasurementSettingsFormControl.SensitivityCoefKey(control.Name);
            var value = ((string)control.Value).ToDouble() ?? 0;
            model.SensitivityCoefs.Add(key, value);
        });
    }

    protected override List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        List<RadioControlViewModel> heaterModeControlGroup = new()
        {
            new(MeasurementSettingsFormControl.HeaterOff, new() { Label = "Нагреватель выключен" }),
            new(MeasurementSettingsFormControl.LinearHeating, new() { Label = "Линейный нагрев" }),
            new(MeasurementSettingsFormControl.TemperatureMaintenance, new() { Label = "Поддержание температуры" })
        };
        list.Add(new RadioGroupControlViewModel(MeasurementSettingsFormControl.HeaterMode,
            heaterModeControlGroup[0].Name, heaterModeControlGroup));

        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.EndTemperature, "0",
            new() { Label = "Конечная температура, °C" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.HeatingRate, "0",
            new() { Label = "Скорость нагрева, °C/сек" }));

        List<RadioControlViewModel> LEDModeControlGroup = new()
        {
            new(MeasurementSettingsFormControl.LedOff, new() { Label = "Светодиод выключен" }),
            new(MeasurementSettingsFormControl.LinearIncreaseCurrent, new() { Label = "Линейное увеличение тока" }),
            new(MeasurementSettingsFormControl.CurrentMaintenance, new() { Label = "Поддержание тока" })
        };
        list.Add(new RadioGroupControlViewModel(MeasurementSettingsFormControl.LEDMode,
            LEDModeControlGroup[0].Name, LEDModeControlGroup));

        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.StartLEDCurrent, 0,
            new() { Label = "Начальный ток, мА" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.EndLEDCurrent, "0",
            new() { Label = "Конечный ток, мА" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.LEDCurrentRate, "0",
            new() { Label = "Скорость роста тока, мА/сек" }));

        List<RadioControlViewModel> PEMModeControlGroup = new()
        {
            new(MeasurementSettingsFormControl.Automatic, new() { Label = "Автоматический" }),
            new(MeasurementSettingsFormControl.Upem, new() { Label = "Управляющее напряжение, В" })
        };
        list.Add(new RadioGroupControlViewModel(MeasurementSettingsFormControl.PEMMode,
            PEMModeControlGroup[0].Name, PEMModeControlGroup));

        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.Ufeu, "0",
            new() { Label = "Напряжение на ФЭУ" }));

        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.LedCAPZeroOffset, "0",
            new() { Label = "Смещение нуля ЦАП" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.LedCAPCoefTransform, "0",
            new() { Label = "Коэффициент преобразования ЦАП" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.CodeChange, "0",
            new() { Label = "Изменение кода ФЭУ" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.TemperatureChange, "0",
            new() { Label = "Изменение температуры" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.ThermocoupleACPZeroOffset, "0",
            new() { Label = "Смещение нуля АЦП" }));
        list.Add(new TextControlViewModel(MeasurementSettingsFormControl.ThermocoupleACPCoefTransform, "0",
            new() { Label = "Коэффициент преобразования АЦП" }));

        for (int i = 0; i < _darkCurrentCodesCount - 1; i++)
        {
            _darkCurrentCodeControls.Add(
                new TextControlViewModel(
                    MeasurementSettingsFormControl.DarkCurrentCodeName(i),
                    "0",
                    new() { Label = $"{_darkCurrentCodesStartValue + _darkCurrentCodesIncrementValue * i}" })
            );
        }

        for (int i = 0; i < _sensitivityCoefsCount - 1; i++)
        {
            _sensitivityCoefControls.Add(new TextControlViewModel(
                MeasurementSettingsFormControl.SensitivityCoefName(i),
                "0",
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
                return MeasurementSettingsFormControl.HeaterOff;
            case 1:
                return MeasurementSettingsFormControl.LinearHeating;
            case 2:
                return MeasurementSettingsFormControl.TemperatureMaintenance;
            default:
                return MeasurementSettingsFormControl.HeaterOff;
        }
    }

    private string GetLEDMode(int mode)
    {
        switch (mode)
        {
            case 0:
                return MeasurementSettingsFormControl.LedOff;
            case 1:
                return MeasurementSettingsFormControl.LinearIncreaseCurrent;
            case 2:
                return MeasurementSettingsFormControl.CurrentMaintenance;
            default:
                return MeasurementSettingsFormControl.LedOff;
        }
    }

    private string GetPEMMode(int mode)
    {
        switch (mode)
        {
            case 1:
                return MeasurementSettingsFormControl.Automatic;
            case 2:
                return MeasurementSettingsFormControl.Upem;
            default:
                return MeasurementSettingsFormControl.Automatic;
        }
    }
}