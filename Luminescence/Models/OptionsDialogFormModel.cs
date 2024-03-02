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

    /** Темновой ток Codes */
    public Dictionary<string, string> DarkCurrentList;

    /** Коэффициенты чувствительности Coefs */
    public Dictionary<string, string> SensitivityCoefList;
}