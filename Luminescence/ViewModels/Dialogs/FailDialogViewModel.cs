using Luminescence.Dialog;
using Luminescence.Models;

namespace Luminescence.ViewModels;

public class FailDialogViewModel : DialogBaseViewModel
{
    public FailModel Model { get; }

    public FailDialogViewModel(FailModel failModel)
    {
        Model = failModel;
    }
}