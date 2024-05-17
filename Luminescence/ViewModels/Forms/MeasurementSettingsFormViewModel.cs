using System.Collections.Generic;
using System.Linq;
using Luminescence.Form;
using Luminescence.Form.ViewModels;
using Luminescence.Models;
using Luminescence.Services;
using Luminescence.Shared.Utils;

namespace Luminescence.ViewModels;

public class MeasurementSettingsFormViewModel : FormViewModel<MeasurementSettingsFormModel>
{
    public List<FormControlBaseViewModel> DarkCurrentCodeControls => _darkCurrentCodeControls;
    public List<FormControlBaseViewModel> SensitivityCoefControls => _sensitivityCoefControls;

    private List<FormControlBaseViewModel> _darkCurrentCodeControls = new();
    private List<FormControlBaseViewModel> _sensitivityCoefControls = new();

    private readonly double _darkCurrentCodesStartValue = 0.5;
    private readonly double _darkCurrentCodesIncrementValue = 0.05;
    private readonly int _darkCurrentCodesCount = 13;
    private readonly double _sensitivityCoefsStartValue = 0.5;
    private readonly double _sensitivityCoefsIncrementValue = 0.05;
    private readonly int _sensitivityCoefsCount = 13;

    public override void FromModel(MeasurementSettingsFormModel model)
    {
        SetControlValue(MeasurementSettingsFormControl.HeaterMode, GetHeaterMode(model.HeaterMode));
        SetControlValue(MeasurementSettingsFormControl.EndTemperature, model.EndTemperature.ToString());
        SetControlValue(MeasurementSettingsFormControl.HeatingRate, model.HeatingRate.ToString());

        SetControlValue(MeasurementSettingsFormControl.LEDMode, GetLEDMode(model.LEDMode));
        SetControlValue(MeasurementSettingsFormControl.StartLEDCurrent, model.StartLEDCurrent);
        SetControlValue(MeasurementSettingsFormControl.EndLEDCurrent, model.EndLEDCurrent.ToString());
        SetControlValue(MeasurementSettingsFormControl.LEDCurrentRate, model.LEDCurrentRate.ToString());

        SetControlValue(MeasurementSettingsFormControl.PEMMode, GetPEMMode(model.PEMMode));
        SetControlValue(MeasurementSettingsFormControl.Ufeu, model.Ufeu.ToString());

        SetControlValue(MeasurementSettingsFormControl.LedCAPZeroOffset, model.LedCAPZeroOffset.ToString());
        SetControlValue(MeasurementSettingsFormControl.LedCAPCoefTransform, model.LedCAPCoefTransform.ToString());
        SetControlValue(MeasurementSettingsFormControl.CodeChange, model.CodeChange.ToString());
        SetControlValue(MeasurementSettingsFormControl.TemperatureChange, model.TemperatureChange.ToString());
        SetControlValue(MeasurementSettingsFormControl.ThermocoupleACPZeroOffset,
            model.ThermocoupleACPZeroOffset.ToString());
        SetControlValue(MeasurementSettingsFormControl.ThermocoupleACPCoefTransform,
            model.ThermocoupleACPCoefTransform.ToString());

        foreach (var darkCurrentCode in model.DarkCurrentCodes)
        {
            SetControlValue(MeasurementSettingsFormControl.DarkCurrentCodeName(darkCurrentCode.Key),
                darkCurrentCode.Value.ToString());
        }

        foreach (var sensitivityCoef in model.SensitivityCoefs)
        {
            SetControlValue(MeasurementSettingsFormControl.SensitivityCoefName(sensitivityCoef.Key),
                sensitivityCoef.Value.ToString());
        }

        SetControlValue(MeasurementSettingsFormControl.Clear, model.Clear);
    }

    protected override void ChangeModel(MeasurementSettingsFormModel model)
    {
        model.HeaterMode =
            MeasurementSettingsFormModel.GetHeaterMode(
                GetControlValue<string>(MeasurementSettingsFormControl.HeaterMode)!);
        model.EndTemperature = GetControlValue<decimal>(MeasurementSettingsFormControl.EndTemperature).ToDouble() ?? 0;
        model.HeatingRate = GetControlValue<decimal>(MeasurementSettingsFormControl.HeatingRate).ToDouble() ?? 0;

        model.LEDMode =
            MeasurementSettingsFormModel.GetLEDMode(GetControlValue<string>(MeasurementSettingsFormControl.LEDMode)!);

        model.StartLEDCurrent =
            GetControlValue<decimal>(MeasurementSettingsFormControl.StartLEDCurrent).ToDouble() ?? 0;
        model.EndLEDCurrent = GetControlValue<decimal>(MeasurementSettingsFormControl.EndLEDCurrent).ToDouble() ?? 0;
        model.LEDCurrentRate = GetControlValue<decimal>(MeasurementSettingsFormControl.LEDCurrentRate).ToDouble() ?? 0;

        model.PEMMode =
            MeasurementSettingsFormModel.GetPEMMode(GetControlValue<string>(MeasurementSettingsFormControl.PEMMode)!);

        model.Ufeu = GetControlValue<decimal>(MeasurementSettingsFormControl.Ufeu).ToDouble() ?? 0;

        model.LedCAPZeroOffset =
            GetControlValue<decimal>(MeasurementSettingsFormControl.LedCAPZeroOffset).ToDouble() ?? 0;
        model.LedCAPCoefTransform =
            GetControlValue<decimal>(MeasurementSettingsFormControl.LedCAPCoefTransform).ToDouble() ?? 0;
        model.CodeChange = GetControlValue<decimal>(MeasurementSettingsFormControl.CodeChange).ToDouble() ?? 0;
        model.TemperatureChange =
            GetControlValue<decimal>(MeasurementSettingsFormControl.TemperatureChange).ToDouble() ?? 0;
        model.ThermocoupleACPZeroOffset =
            GetControlValue<decimal>(MeasurementSettingsFormControl.ThermocoupleACPZeroOffset).ToDouble() ?? 0;
        model.ThermocoupleACPCoefTransform =
            GetControlValue<decimal>(MeasurementSettingsFormControl.ThermocoupleACPCoefTransform).ToDouble() ?? 0;

        model.DarkCurrentCodes = new();
        _darkCurrentCodeControls.ForEach(control =>
        {
            var key = MeasurementSettingsFormControl.DarkCurrentCodeKey(control.Name);
            var value = ((decimal)control.Value).ToDouble() ?? 0;
            model.DarkCurrentCodes.Add(key, value);
        });

        model.SensitivityCoefs = new();
        _sensitivityCoefControls.ForEach(control =>
        {
            var key = MeasurementSettingsFormControl.SensitivityCoefKey(control.Name);
            var value = ((decimal)control.Value).ToDouble() ?? 0;
            model.SensitivityCoefs.Add(key, value);
        });
        model.Clear = GetControlValue<bool>(MeasurementSettingsFormControl.Clear);
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

        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.EndTemperature, 0,
            new()
            {
                Label = "Конечная температура, °C",
                Spinner = new() { Minimum = 0, Maximum = 700, Increment = 10 }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.HeatingRate, 0,
            new()
            {
                Label = "Скорость нагрева, °C/сек",
                Spinner = new() { Minimum = new(0.1), Maximum = 20, Increment = new(0.1) }
            }));

        List<RadioControlViewModel> LEDModeControlGroup = new()
        {
            new(MeasurementSettingsFormControl.LedOff, new() { Label = "Светодиод выключен" }),
            new(MeasurementSettingsFormControl.LinearIncreaseCurrent, new() { Label = "Линейное увеличение тока" }),
            new(MeasurementSettingsFormControl.CurrentMaintenance, new() { Label = "Поддержание тока" })
        };
        list.Add(new RadioGroupControlViewModel(MeasurementSettingsFormControl.LEDMode,
            LEDModeControlGroup[0].Name, LEDModeControlGroup));

        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.StartLEDCurrent, 0,
            new()
            {
                Label = "Начальный ток, мА",
                Spinner = new() { Minimum = 0, Maximum = 1000, Increment = 50 }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.EndLEDCurrent, 0,
            new()
            {
                Label = "Конечный ток, мА",
                Spinner = new() { Minimum = 0, Maximum = 1000, Increment = 50 }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.LEDCurrentRate, 0,
            new()
            {
                Label = "Скорость роста тока, мА/сек",
                Spinner = new() { Minimum = new(0.1), Maximum = 20, Increment = new(0.1) }
            }));

        List<RadioControlViewModel> PEMModeControlGroup = new()
        {
            new(MeasurementSettingsFormControl.Automatic, new() { Label = "Автоматический" }),
            new(MeasurementSettingsFormControl.Upem, new() { Label = "Управляющее напряжение, В" })
        };
        list.Add(new RadioGroupControlViewModel(MeasurementSettingsFormControl.PEMMode,
            PEMModeControlGroup[0].Name, PEMModeControlGroup));

        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.Ufeu, 0,
            new()
            {
                Label = "Напряжение на ФЭУ, В",
                Spinner = new() { Minimum = new(0.5), Maximum = 20, Increment = new(0.05) }
            }));

        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.LedCAPZeroOffset, 0,
            new()
            {
                Label = "Смещение нуля ЦАП",
                Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.LedCAPCoefTransform, 0,
            new()
            {
                Label = "Коэффициент преобразования ЦАП",
                Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.CodeChange, 0,
            new()
            {
                Label = "Изменение кода ФЭУ",
                Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.TemperatureChange, 0,
            new()
            {
                Label = "Изменение температуры",
                Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.ThermocoupleACPZeroOffset, 0,
            new()
            {
                Label = "Смещение нуля АЦП",
                Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
            }));
        list.Add(new NumericControlViewModel(MeasurementSettingsFormControl.ThermocoupleACPCoefTransform, 0,
            new()
            {
                Label = "Коэффициент преобразования АЦП",
                Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
            }));
        for (int i = 0; i < _darkCurrentCodesCount - 1; i++)
        {
            _darkCurrentCodeControls.Add(
                new NumericControlViewModel(
                    MeasurementSettingsFormControl.DarkCurrentCodeName(i),
                    0,
                    new()
                    {
                        Label = $"{_darkCurrentCodesStartValue + _darkCurrentCodesIncrementValue * i}",
                        Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
                    })
            );
        }

        for (int i = 0; i < _sensitivityCoefsCount - 1; i++)
        {
            _sensitivityCoefControls.Add(new NumericControlViewModel(
                MeasurementSettingsFormControl.SensitivityCoefName(i),
                0,
                new()
                {
                    Label = $"{_sensitivityCoefsStartValue + _sensitivityCoefsIncrementValue * i}",
                    Spinner = new() { Minimum = 0, Maximum = 1, Increment = new(0.1) }
                }));
        }

        list.Add(new CheckboxControlViewModel(MeasurementSettingsFormControl.Clear, true,
            new() { Label = "Очищение графиков перед измерением" }));

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