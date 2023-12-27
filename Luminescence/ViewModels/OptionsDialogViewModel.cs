using Luminescence.Dialog;
using Luminescence.Services;

namespace Luminescence.ViewModels;

public class OptionsDialogViewModel : DialogBaseViewModel
{
    public OptionsDialogFormViewModel Form { get; }

    private OptionsDialogFormService _optionsDialogFormService;

    public OptionsDialogViewModel(OptionsDialogFormService optionsDialogFormService)
    {
        _optionsDialogFormService = optionsDialogFormService;

        Form = new OptionsDialogFormViewModel();

        _optionsDialogFormService.Initialize(Form);
    }
}