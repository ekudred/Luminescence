using Luminescence.Dialog;
using Luminescence.Services;

namespace Luminescence.ViewModels;

public class SettingsDialogViewModel : DialogBaseViewModel
{
    public MeasurementSettingsFormViewModel Form { get; }

    public SettingsDialogViewModel(MeasurementSettingsFormService measurementSettingsFormService)
    {
        Form = new MeasurementSettingsFormViewModel();

        measurementSettingsFormService.Initialize(Form);
    }

    public override void OnInitialize(DialogBaseParam? param)
    {
    }
}