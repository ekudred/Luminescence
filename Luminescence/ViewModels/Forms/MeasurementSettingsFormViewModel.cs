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

    public void Apply()
    {
        UpdateInitialModel();
    }

    protected override void UpdateModel(MeasurementSettingsFormModel model)
    {
        string activeControlGroup0Name = ((RadioControlViewModel)GetControl("Group0").Value).Name;
        model.HeaterOff = activeControlGroup0Name == "HeaterOff";
        model.LinearHeating = activeControlGroup0Name == "LinearHeating";
        model.TemperatureMaintenance = activeControlGroup0Name == "TemperatureMaintenance";

        // model.EndTemperature = int.Parse((string)GetControl("EndTemperature").Value ?? "0");
        // model.HeatingRate = int.Parse((string)GetControl("HeatingRate").Value ?? "0");

        string activeControlGroup1Name = ((RadioControlViewModel)GetControl("Group1").Value).Name;
        model.LedOff = activeControlGroup1Name == "LedOff";
        model.LinearIncreaseCurrent = activeControlGroup1Name == "LinearIncreaseCurrent";
        model.CurrentMaintenance = activeControlGroup1Name == "CurrentMaintenance";

        // model.StartLEDCurrent = int.Parse((string)GetControl("StartLEDCurrent").Value ?? "0");
        // model.EndLEDCurrent = int.Parse((string)GetControl("EndLEDCurrent").Value ?? "0");
        // model.LEDCurrentRate = int.Parse((string)GetControl("LEDCurrentRate").Value ?? "0");

        string activeControlGroup2Name = ((RadioControlViewModel)GetControl("Group2").Value).Name;
        model.Automatic = activeControlGroup2Name == "Automatic";
        model.Upem = activeControlGroup2Name == "Upem";

        // model.Ufeu = int.Parse((string)GetControl("Ufeu").Value ?? "0");
        //
        // model.LedCAPZeroOffset = int.Parse((string)GetControl("LedCAPZeroOffset").Value ?? "0");
        // model.LedCAPCoefTransform = int.Parse((string)GetControl("LedCAPCoefTransform").Value ?? "0");
        // model.CodeChange = int.Parse((string)GetControl("CodeChange").Value ?? "0");
        // model.TemperatureChange = int.Parse((string)GetControl("TemperatureChange").Value ?? "0");
        // model.ThermocoupleACPZeroOffset = int.Parse((string)GetControl("ThermocoupleACPZeroOffset").Value ?? "0");
        // model.ThermocoupleACPCoefTransform = int.Parse((string)GetControl("ThermocoupleACPCoefTransform").Value ?? "0");

        model.DarkCurrentCodes = new();

        for (int i = 0; i < _darkCurrentCodesCount - 1; i++)
        {
            var control = (TextControlViewModel)GetControl("DarkCurrentCode" + i);
            model.DarkCurrentCodes.Add(control.Label, (string)control.Value);
        }

        model.SensitivityCoefs = new();

        for (int i = 0; i < _sensitivityCoefsCount - 1; i++)
        {
            var control = (TextControlViewModel)GetControl("SensitivityCoef" + i);
            model.SensitivityCoefs.Add(control.Label, (string)control.Value);
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

        list.Add(new TextControlViewModel("EndTemperature", "0", new() { Label = "Конечная температура, °C" }));
        list.Add(new TextControlViewModel("HeatingRate", "0", new() { Label = "Скорость нагрева, °C/сек" }));

        List<RadioControlViewModel> group1 = new()
        {
            new("LedOff", new() { Label = "Светодиод выключен" }),
            new("LinearIncreaseCurrent", new() { Label = "Линейное увеличение тока" }),
            new("CurrentMaintenance", new() { Label = "Поддержание тока" })
        };
        list.Add(new RadioGroupControlViewModel("Group1", 0, group1));

        list.Add(new TextControlViewModel("StartLEDCurrent", "0", new() { Label = "Начальный ток, мА" }));
        list.Add(new TextControlViewModel("EndLEDCurrent", "0", new() { Label = "Конечный ток, мА" }));
        list.Add(new TextControlViewModel("LEDCurrentRate", "0", new() { Label = "Скорость роста тока, мА/сек" }));

        List<RadioControlViewModel> group2 = new()
        {
            new("Automatic", new() { Label = "Автоматический" }),
            new("Upem", new() { Label = "Управляющее напряжение, В" })
        };
        list.Add(new RadioGroupControlViewModel("Group2", 0, group2));

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