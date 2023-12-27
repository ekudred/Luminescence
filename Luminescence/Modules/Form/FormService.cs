using Luminescence.Form.ViewModels;

namespace Luminescence.Form;

public abstract class FormService<TFormViewModel, TFormModel>
    where TFormViewModel : FormViewModel<TFormModel>
    where TFormModel : FormBaseModel
{
    public void Initialize(TFormViewModel model)
    {
        model.Initialize();
    }
}