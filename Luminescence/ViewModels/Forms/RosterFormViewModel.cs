using System.Collections.Generic;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.Form.ViewModels;
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

    private double _temperature = 0;
    private double _voltagePmt = 0;
    private double _current = 0;
    private double _code = 404;
    private string _description = "";

    public RosterFormViewModel() : base(new RosterFormModel())
    {
    }

    protected override void ChangeModel(RosterFormModel model)
    {
        model.HeaterOff = ((RadioControlViewModel)GetControl("HeaterOff")).Value;
        model.LinearHeating = ((RadioControlViewModel)GetControl("LinearHeating")).Value;
        model.TemperatureMaintenance = ((RadioControlViewModel)GetControl("TemperatureMaintenance")).Value;
        model.FinalTemperature = ((TextControlViewModel)GetControl("FinalTemperature")).Value;
        model.HeatingRate = ((TextControlViewModel)GetControl("HeatingRate")).Value;

        model.LedOff = ((RadioControlViewModel)GetControl("LedOff")).Value;
        model.LinearIncreaseCurrent = ((RadioControlViewModel)GetControl("LinearIncreaseCurrent")).Value;
        model.CurrentMaintenance = ((RadioControlViewModel)GetControl("CurrentMaintenance")).Value;
        model.InitialCurrent = ((TextControlViewModel)GetControl("InitialCurrent")).Value;
        model.FinalCurrent = ((TextControlViewModel)GetControl("FinalCurrent")).Value;
        model.CurrentRiseRate = ((TextControlViewModel)GetControl("CurrentRiseRate")).Value;

        model.Automatic = ((RadioControlViewModel)GetControl("Automatic")).Value;
        model.ControlVoltage = ((RadioControlViewModel)GetControl("ControlVoltage")).Value;
        model.FinalCurrent = ((TextControlViewModel)GetControl("Label")).Value;
    }

    protected override List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        list.Add(new RadioControlViewModel("HeaterOff", true,
            new RadioControlOptions { Label = "Нагреватель выключен" }));
        list.Add(new RadioControlViewModel("LinearHeating", false,
            new RadioControlOptions { Label = "Линейный нагрев" }));
        list.Add(new RadioControlViewModel("TemperatureMaintenance", false,
            new RadioControlOptions { Label = "Поддержание температуры" }));
        list.Add(new TextControlViewModel("FinalTemperature", "",
            new TextControlOptions { Label = "Конечная температура, °C" }));
        list.Add(new TextControlViewModel("HeatingRate", "",
            new TextControlOptions { Label = "Скорость нагрева, °C/сек" }));

        list.Add(new RadioControlViewModel("LedOff", true,
            new RadioControlOptions { Label = "Светодиод выключен" }));
        list.Add(new RadioControlViewModel("LinearIncreaseCurrent", false,
            new RadioControlOptions { Label = "Линейное увеличение тока" }));
        list.Add(new RadioControlViewModel("CurrentMaintenance", false,
            new RadioControlOptions { Label = "Поддержание тока" }));
        list.Add(new TextControlViewModel("InitialCurrent", "",
            new TextControlOptions { Label = "Скорость роста тока, мА/сек" }));
        list.Add(new TextControlViewModel("FinalCurrent", "",
            new TextControlOptions { Label = "Начальный ток, мА" }));
        list.Add(new TextControlViewModel("CurrentRiseRate", "",
            new TextControlOptions { Label = "Конечный ток, мА" }));

        list.Add(new RadioControlViewModel("Automatic", true,
            new RadioControlOptions { Label = "Автоматический" }));
        list.Add(new RadioControlViewModel("ControlVoltage", false,
            new RadioControlOptions { Label = "Управляющее напряжение, В" }));
        list.Add(new TextControlViewModel("Label", "",
            new TextControlOptions { Label = "Label" }));

        return list;
    }

    protected override void OnInitialize()
    {
    }
}