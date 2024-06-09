using System;
using System.Reactive.Linq;
using Luminescence.Form.ViewModels;

namespace Luminescence.Form;

public abstract class FormService<TFormViewModel, TFormModel>
    where TFormViewModel : FormViewModel<TFormModel>
    where TFormModel : FormModel
{
    public virtual void Initialize(TFormViewModel formViewModel)
    {
        formViewModel.Initialized = false;

        formViewModel.Initialize();

        Fill(formViewModel)
            .Subscribe(formModel =>
            {
                if (formModel != null)
                {
                    formViewModel.FromModel(formModel);
                    formViewModel.UpdateModel();
                    formViewModel.UpdateInitialModel();
                    formViewModel.CheckChanges();
                }

                formViewModel.Initialized = true;
            });
    }

    protected virtual IObservable<TFormModel?> Fill(TFormViewModel model)
    {
        return Observable.Return<TFormModel?>(null);
    }
}