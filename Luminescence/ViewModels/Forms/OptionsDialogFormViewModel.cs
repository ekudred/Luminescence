using System.Collections.Generic;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.Form.ViewModels;

namespace Luminescence.ViewModels;

public class OptionsDialogFormViewModel : FormViewModel<OptionsDialogFormModel>
{
    public OptionsDialogFormViewModel() : base(new OptionsDialogFormModel())
    {
    }

    protected override void ChangeModel(OptionsDialogFormModel model)
    {
        model.LedCAPZeroOffset = ((TextControlViewModel)GetControl("LedCAPZeroOffset")).Value;
        model.LedCAPCoefTransform = ((TextControlViewModel)GetControl("LedCAPCoefTransform")).Value;
        model.CodeChange = ((TextControlViewModel)GetControl("CodeChange")).Value;
        model.TemperatureChange = ((TextControlViewModel)GetControl("TemperatureChange")).Value;
        model.ThermocoupleACPZeroOffset = ((TextControlViewModel)GetControl("ThermocoupleACPZeroOffset")).Value;
        model.ThermocoupleACPCoefTransform = ((TextControlViewModel)GetControl("ThermocoupleACPCoefTransform")).Value;
        model.ClearCharts = ((CheckboxControlViewModel)GetControl("ClearCharts")).Value;

        model.DarkCurrentList = new List<DarkCurrentItem>();

        for (int i = 0; i < 12; i++)
        {
            var control = (TextControlViewModel)GetControl("Code" + i);
            model.DarkCurrentList.Add(new DarkCurrentItem(control.Label, control.Value));
        }

        model.SensitivityCoefList = new List<SensitivityCoefItem>();

        for (int i = 0; i < 12; i++)
        {
            var control = (TextControlViewModel)GetControl("Coef" + i);
            model.SensitivityCoefList.Add(new SensitivityCoefItem(control.Label, control.Value));
        }
    }

    protected override List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        list.Add(new TextControlViewModel("LedCAPZeroOffset", "",
            new TextControlOptions { Label = "Смещение нуля ЦАП" }));
        list.Add(new TextControlViewModel("LedCAPCoefTransform", "",
            new TextControlOptions { Label = "Коэффициент преобразования ЦАП" }));
        list.Add(new TextControlViewModel("CodeChange", "",
            new TextControlOptions { Label = "Изменение кода ФЭУ" }));
        list.Add(new TextControlViewModel("TemperatureChange", "",
            new TextControlOptions { Label = "Изменение температуры" }));
        list.Add(new TextControlViewModel("ThermocoupleACPZeroOffset", "",
            new TextControlOptions { Label = "Смещение нуля АЦП" }));
        list.Add(new TextControlViewModel("ThermocoupleACPCoefTransform", "",
            new TextControlOptions { Label = "Коэффициент преобразования АЦП" }));
        list.Add(new CheckboxControlViewModel("ClearCharts", true,
            new CheckboxControlOptions { Label = "Очищать графики при запуске измерения" }));

        for (int i = 0; i < 12; i++)
        {
            list.Add(new TextControlViewModel("Code" + i, "0",
                new TextControlOptions { Label = string.Format("{0}", 0.50 + 0.05 * i) }));
        }

        for (int i = 0; i < 12; i++)
        {
            list.Add(new TextControlViewModel("Coef" + i, "0",
                new TextControlOptions { Label = string.Format("{0}", 0.50 + 0.05 * i) }));
        }

        return list;
    }

    protected override void OnInitialize()
    {
    }
}