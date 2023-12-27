using Luminescence.Form;

namespace Luminescence.Models;

public class RosterFormModel : FormBaseModel
{
    public bool HeaterOff;
    public bool LinearHeating;
    public bool TemperatureMaintenance;
    public string FinalTemperature;
    public string HeatingRate;
    
    public bool LedOff;
    public bool LinearIncreaseCurrent;
    public bool CurrentMaintenance;
    public string InitialCurrent;
    public string FinalCurrent;
    public string CurrentRiseRate;

    public bool Automatic;
    public bool ControlVoltage;
    public string Label;
}