using System.Collections.Generic;
using Luminescence.Form;

namespace Luminescence.Models;

public class OptionsDialogFormModel : FormBaseModel
{
    public string LedCAPZeroOffset;
    public string LedCAPCoefTransform;
    public string CodeChange;
    public string TemperatureChange;
    public string ThermocoupleACPZeroOffset;
    public string ThermocoupleACPCoefTransform;
    public bool ClearCharts;

    public List<DarkCurrentItem> DarkCurrentList;

    public List<SensitivityCoefItem> SensitivityCoefList;
}

public class DarkCurrentItem
{
    public string Direction;
    public string Code;

    public DarkCurrentItem(string direction, string code)
    {
        Direction = direction;
        Code = code;
    }
}

public class SensitivityCoefItem
{
    public string Direction;
    public string Coef;

    public SensitivityCoefItem(string direction, string coef)
    {
        Direction = direction;
        Coef = coef;
    }
}