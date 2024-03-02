using Luminescence.Dialog;
using Luminescence.Models;

namespace Luminescence.ViewModels;

public class FailDialogViewModel : DialogBaseViewModel
{
    public FailModel Model { get; set; }

    public FailDialogViewModel(FailModel failModel)
    {
        Model = failModel;
    }

    public override void OnInitialize(DialogBaseParam? param)
    {
    }
}