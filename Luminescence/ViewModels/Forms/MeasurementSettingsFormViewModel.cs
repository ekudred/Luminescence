using System.Collections.Generic;
using System.Linq;
using Luminescence.Form.ViewModels;
using Luminescence.Models;

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

    public MeasurementSettingsFormViewModel() : base(new())
    {
    }

    protected override void ChangeModel(MeasurementSettingsFormModel model)
    {
        string activeControlGroup0Name = ((RadioGroupControlViewModel)GetControl("Group0")).Value.Name;
        model.HeaterOff = activeControlGroup0Name == "HeaterOff";
        model.LinearHeating = activeControlGroup0Name == "LinearHeating";
        model.TemperatureMaintenance = activeControlGroup0Name == "TemperatureMaintenance";

        // model.EndTemperature = ((TextControlViewModel)GetControl("EndTemperature")).Value;
        // model.HeatingRate = ((TextControlViewModel)GetControl("HeatingRate")).Value;

        string activeControlGroup1Name = ((RadioGroupControlViewModel)GetControl("Group1")).Value.Name;
        model.LedOff = activeControlGroup1Name == "LedOff";
        model.LinearIncreaseCurrent = activeControlGroup1Name == "LinearIncreaseCurrent";
        model.CurrentMaintenance = activeControlGroup1Name == "CurrentMaintenance";

        // model.StartLEDCurrent = ((TextControlViewModel)GetControl("StartLEDCurrent")).Value;
        // model.EndLEDCurrent = ((TextControlViewModel)GetControl("EndLEDCurrent")).Value;
        // model.LEDCurrentRate = ((TextControlViewModel)GetControl("LEDCurrentRate")).Value;

        string activeControlGroup2Name = ((RadioGroupControlViewModel)GetControl("Group2")).Value.Name;
        model.Automatic = activeControlGroup2Name == "Automatic";
        model.Upem = activeControlGroup2Name == "Upem";
        // model.EndLEDCurrent = ((TextControlViewModel)GetControl("Ufeu")).Value;

        model.LedCAPZeroOffset = ((TextControlViewModel)GetControl("LedCAPZeroOffset")).Value;
        model.LedCAPCoefTransform = ((TextControlViewModel)GetControl("LedCAPCoefTransform")).Value;
        model.CodeChange = ((TextControlViewModel)GetControl("CodeChange")).Value;
        model.TemperatureChange = ((TextControlViewModel)GetControl("TemperatureChange")).Value;
        model.ThermocoupleACPZeroOffset = ((TextControlViewModel)GetControl("ThermocoupleACPZeroOffset")).Value;
        model.ThermocoupleACPCoefTransform = ((TextControlViewModel)GetControl("ThermocoupleACPCoefTransform")).Value;

        model.DarkCurrentCodes = new();

        for (int i = 0; i < _darkCurrentCodesCount - 1; i++)
        {
            var control = (TextControlViewModel)GetControl("DarkCurrentCode" + i);
            model.DarkCurrentCodes.Add(control.Label, control.Value);
        }

        model.SensitivityCoefs = new();

        for (int i = 0; i < _sensitivityCoefsCount - 1; i++)
        {
            var control = (TextControlViewModel)GetControl("SensitivityCoef" + i);
            model.SensitivityCoefs.Add(control.Label, control.Value);
        }
    }

    protected override List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        List<RadioControlViewModel> group0 = new()
        {
            new("HeaterOff", new() { Label = "Нагреватель выключен" }),
            new("LinearHeating", new() { Label = "Линейный нагрев" }),
            new("TemperatureMaintenance", new() { Label = "Поддержание температуры" })
        };
        list.Add(new RadioGroupControlViewModel("Group0", 0, group0));

        list.Add(new TextControlViewModel("EndTemperature", "", new() { Label = "Конечная температура, °C" }));
        list.Add(new TextControlViewModel("HeatingRate", "", new() { Label = "Скорость нагрева, °C/сек" }));

        List<RadioControlViewModel> group1 = new()
        {
            new("LedOff", new() { Label = "Светодиод выключен" }),
            new("LinearIncreaseCurrent", new() { Label = "Линейное увеличение тока" }),
            new("CurrentMaintenance", new() { Label = "Поддержание тока" })
        };
        list.Add(new RadioGroupControlViewModel("Group1", 0, group1));

        list.Add(new TextControlViewModel("StartLEDCurrent", "", new() { Label = "Начальный ток, мА" }));
        list.Add(new TextControlViewModel("EndLEDCurrent", "", new() { Label = "Конечный ток, мА" }));
        list.Add(new TextControlViewModel("LEDCurrentRate", "", new() { Label = "Скорость роста тока, мА/сек" }));

        List<RadioControlViewModel> group2 = new()
        {
            new("Automatic", new() { Label = "Автоматический" }),
            new("Upem", new() { Label = "Управляющее напряжение, В" })
        };
        list.Add(new RadioGroupControlViewModel("Group2", 0, group2));

        list.Add(new TextControlViewModel("Ufeu", "", new() { Label = "Напряжение на ФЭУ" }));

        list.Add(new TextControlViewModel("LedCAPZeroOffset", "", new() { Label = "Смещение нуля ЦАП" }));
        list.Add(new TextControlViewModel("LedCAPCoefTransform", "",
            new() { Label = "Коэффициент преобразования ЦАП" }));
        list.Add(new TextControlViewModel("CodeChange", "", new() { Label = "Изменение кода ФЭУ" }));
        list.Add(new TextControlViewModel("TemperatureChange", "", new() { Label = "Изменение температуры" }));
        list.Add(new TextControlViewModel("ThermocoupleACPZeroOffset", "", new() { Label = "Смещение нуля АЦП" }));
        list.Add(new TextControlViewModel("ThermocoupleACPCoefTransform", "",
            new() { Label = "Коэффициент преобразования АЦП" }));

        for (int i = 0; i < _darkCurrentCodesCount - 1; i++)
        {
            _darkCurrentCodeControls.Add(new TextControlViewModel("DarkCurrentCode" + i, "0",
                new() { Label = $"{_darkCurrentCodesStartValue + _darkCurrentCodesIncrementValue * i}", }));
        }

        for (int i = 0; i < _sensitivityCoefsCount - 1; i++)
        {
            _sensitivityCoefControls.Add(new TextControlViewModel("SensitivityCoef" + i, "0",
                new() { Label = $"{_sensitivityCoefsStartValue + _sensitivityCoefsIncrementValue * i}" }));
        }

        return list
            .Concat(_darkCurrentCodeControls)
            .Concat(_sensitivityCoefControls)
            .ToList();
    }

    protected override void OnInitialize()
    {
    }
}