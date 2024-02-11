using System.Collections.Generic;
using Luminescence.Form;

namespace Luminescence.Models;

public class OptionsDialogFormModel : FormBaseModel
{
    /** Смещение нуля ЦАП */
    public string LedCAPZeroOffset;
    /** Коэффициент преобразования ЦАП */
    public string LedCAPCoefTransform;
    /** Изменение кода ФЭУ */
    public string CodeChange;
    /** Изменение температуры */
    public string TemperatureChange;
    /** Смещение нуля АЦП */
    public string ThermocoupleACPZeroOffset;
    /** Коэффициент преобразования АЦП */
    public string ThermocoupleACPCoefTransform;
    /** * Очищать графики при запуске измерения */
    public bool ClearCharts;
    /** Темновой ток Codes */
    public List<DarkCurrentItem> DarkCurrentList;
    /** Коэффициенты чувствительности Coefs */
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