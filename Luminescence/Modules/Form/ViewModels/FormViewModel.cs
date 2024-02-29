using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Luminescence.ViewModels;

namespace Luminescence.Form.ViewModels;

public class FormViewModel<TFormModel> : BaseViewModel
    where TFormModel : FormBaseModel
{
    public Dictionary<string, FormControlBaseViewModel> Controls => _controls;

    private TFormModel _model;

    private Dictionary<string, FormControlBaseViewModel> _controls;

    public FormViewModel(TFormModel model)
    {
        _model = model;
        _controls = new Dictionary<string, FormControlBaseViewModel>();
    }

    public void Initialize()
    {
        GetControls(new List<FormControlBaseViewModel>())
            .ForEach(control => _controls.Add(control.Name, control));

        ChangeModel(_model);
        OnInitialize();
    }

    public TFormModel ToModel()
    {
        ChangeModel(_model);

        return _model;
    }

    public FormControlBaseViewModel GetControl(string name)
    {
        FormControlBaseViewModel value;

        if (_controls.TryGetValue(name, out value))
        {
            return value;
        }

        return value;
    }

    protected virtual void ChangeModel(TFormModel model)
    {
        throw new NotImplementedException();
    }

    protected virtual List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnInitialize()
    {
        throw new NotImplementedException();
    }
}