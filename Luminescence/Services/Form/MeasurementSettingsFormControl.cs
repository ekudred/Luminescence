using Luminescence.Shared.Utils;

namespace Luminescence.Services;

public static class MeasurementSettingsFormControl
{
    public const string HeaterMode = "HeaterMode";
    public const string HeaterOff = "HeaterOff";
    public const string LinearHeating = "LinearHeating";
    public const string TemperatureMaintenance = "TemperatureMaintenance";
    public const string EndTemperature = "EndTemperature";
    public const string HeatingRate = "HeatingRate";
    public const string LEDMode = "HeaterOff";
    public const string LedOff = "LedOff";
    public const string LinearIncreaseCurrent = "LinearIncreaseCurrent";
    public const string CurrentMaintenance = "CurrentMaintenance";
    public const string StartLEDCurrent = "StartLEDCurrent";
    public const string EndLEDCurrent = "EndLEDCurrent";
    public const string LEDCurrentRate = "LEDCurrentRate";
    public const string PEMMode = "PEMMode";
    public const string Automatic = "Automatic";
    public const string Upem = "Upem";
    public const string Ufeu = "Ufeu";
    public const string LedCAPZeroOffset = "LedCAPZeroOffset";
    public const string LedCAPCoefTransform = "LedCAPCoefTransform";
    public const string CodeChange = "CodeChange";
    public const string TemperatureChange = "TemperatureChange";
    public const string ThermocoupleACPZeroOffset = "ThermocoupleACPZeroOffset";
    public const string ThermocoupleACPCoefTransform = "ThermocoupleACPCoefTransform";
    public const string DarkCurrentCode = "DarkCurrentCode";
    public const double DarkCurrentCodesStartValue = 0.5;
    public const double DarkCurrentCodesIncrementValue = 0.05;
    public const int DarkCurrentCodesCount = 14;
    public const double SensitivityCoefsStartValue = 0.5;
    public const double SensitivityCoefsIncrementValue = 0.05;
    public const int SensitivityCoefsCount = 14;
    public static string DarkCurrentCodeName(int i) => $"{DarkCurrentCode}{i}";
    public static int DarkCurrentCodeKey(string code) => code.Replace(DarkCurrentCode, string.Empty).ToInt();
    public const string SensitivityCoef = "SensitivityCoef";
    public static string SensitivityCoefName(int i) => $"{SensitivityCoef}{i}";
    public static int SensitivityCoefKey(string coef) => coef.Replace(SensitivityCoef, string.Empty).ToInt();
    public const string Clear = "Clear";
}