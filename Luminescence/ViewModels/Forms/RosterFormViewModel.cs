using System;
using System.Collections.Generic;
using DynamicData;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.Form.ViewModels;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class RosterFormViewModel : FormViewModel<RosterFormModel>
{
    public double Temperature
    {
        get => _temperature;
        set => this.RaiseAndSetIfChanged(ref _temperature, value);
    }

    public double VoltagePmt
    {
        get => _voltagePmt;
        set => this.RaiseAndSetIfChanged(ref _voltagePmt, value);
    }

    public double Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }

    public double Code
    {
        get => _code;
        set => this.RaiseAndSetIfChanged(ref _code, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string Test
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }

    private double _temperature = 0;
    private double _voltagePmt = 0;
    private double _current = 0;
    private double _code = 404;
    private string _description = "";
    private string _test = "";

    public RosterFormViewModel() : base(new RosterFormModel())
    {
    }

    protected override void ChangeModel(RosterFormModel model)
    {
        // model.HeaterOff = ((RadioControlViewModel)GetControl("HeaterOff")).Value;
        // model.LinearHeating = ((RadioControlViewModel)GetControl("LinearHeating")).Value;
        // model.TemperatureMaintenance = ((RadioControlViewModel)GetControl("TemperatureMaintenance")).Value;
        // model.EndTemperature = ((TextControlViewModel)GetControl("EndTemperature")).Value;
        // model.HeatRate = ((TextControlViewModel)GetControl("HeatRate")).Value;
        //
        // model.LedOff = ((RadioControlViewModel)GetControl("LedOff")).Value;
        // model.LinearIncreaseCurrent = ((RadioControlViewModel)GetControl("LinearIncreaseCurrent")).Value;
        // model.CurrentMaintenance = ((RadioControlViewModel)GetControl("CurrentMaintenance")).Value;
        // model.StartLEDCurrent = ((TextControlViewModel)GetControl("StartLEDCurrent")).Value;
        // model.EndLEDCurrent = ((TextControlViewModel)GetControl("EndLEDCurrent")).Value;
        // model.LEDCurrentRate = ((TextControlViewModel)GetControl("LEDCurrentRate")).Value;
        //
        // model.Automatic = ((RadioControlViewModel)GetControl("Automatic")).Value;
        // model.Upem = ((RadioControlViewModel)GetControl("Upem")).Value;
        // model.EndLEDCurrent = ((TextControlViewModel)GetControl("Label")).Value;
    }

    protected override List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        List<RadioControlViewModel> group0 = new List<RadioControlViewModel>();
        group0.Add(new RadioControlViewModel("HeaterOff", new RadioControlOptions { Label = "Нагреватель выключен" }));
        group0.Add(new RadioControlViewModel("LinearHeating", new RadioControlOptions { Label = "Линейный нагрев" }));
        group0.Add(new RadioControlViewModel("TemperatureMaintenance",
            new RadioControlOptions { Label = "Поддержание температуры" }));

        list.Add(new RadioGroupControlViewModel("Group0", 0, group0));

        list.Add(new TextControlViewModel("EndTemperature", "",
            new TextControlOptions { Label = "Конечная температура, °C" }));
        list.Add(new TextControlViewModel("HeatRate", "",
            new TextControlOptions { Label = "Скорость нагрева, °C/сек" }));

        List<RadioControlViewModel> group1 = new List<RadioControlViewModel>();
        group1.Add(new RadioControlViewModel("LedOff", new RadioControlOptions { Label = "Светодиод выключен" }));
        group1.Add(new RadioControlViewModel("LinearIncreaseCurrent",
            new RadioControlOptions { Label = "Линейное увеличение тока" }));
        group1.Add(new RadioControlViewModel("CurrentMaintenance",
            new RadioControlOptions { Label = "Поддержание тока" }));

        list.Add(new RadioGroupControlViewModel("Group1", 0, group1));

        list.Add(new TextControlViewModel("StartLEDCurrent", "",
            new TextControlOptions { Label = "Начальный ток, мА" }));
        list.Add(new TextControlViewModel("EndLEDCurrent", "",
            new TextControlOptions { Label = "Конечный ток, мА" }));
        list.Add(new TextControlViewModel("LEDCurrentRate", "",
            new TextControlOptions { Label = "Скорость роста тока, мА/сек" }));

        List<RadioControlViewModel> group2 = new List<RadioControlViewModel>();
        group2.Add(new RadioControlViewModel("Automatic", new RadioControlOptions { Label = "Автоматический" }));
        group2.Add(new RadioControlViewModel("Upem", new RadioControlOptions { Label = "Управляющее напряжение, В" }));

        list.Add(new RadioGroupControlViewModel("Group2", 0, group2));

        list.Add(new TextControlViewModel("Label", "",
            new TextControlOptions { Label = "Label" }));

        return list;
    }

    protected override void OnInitialize()
    {
    }
}