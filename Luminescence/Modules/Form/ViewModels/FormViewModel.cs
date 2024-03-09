using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luminescence.ViewModels;

namespace Luminescence.Form.ViewModels;

public class FormViewModel<TFormModel> : BaseViewModel
    where TFormModel : FormBaseModel
{
    public Dictionary<string, FormControlBaseViewModel> Controls => _controls;
    public readonly Subject<bool> FormChanged = new();

    public Subject<object> destroyForm;

    private TFormModel _model;
    private TFormModel _initialModel;

    private Dictionary<string, FormControlBaseViewModel> _controls = new();

    public void Initialize()
    {
        destroyForm = new();

        GetControls(new())
            .ForEach(control => _controls.Add(control.Name, control));

        _model = Activator.CreateInstance<TFormModel>();
        UpdateModel(_model);
        SetInitialModel();

        Controls
            .Select(control => control.Value.ValueChanges).Merge()
            .TakeUntil(destroyForm)
            .Subscribe(_ => { FormChanged.OnNext(!ToModel().Equals(_initialModel)); });

        OnInitialize();
    }

    public void Destroy()
    {
        if (destroyForm == null)
        {
            return;
        }

        foreach (var control in Controls)
        {
            control.Value.Destroy();
        }

        destroyForm.OnNext(null!);
        destroyForm.OnCompleted();
        destroyForm = null;
    }

    public FormControlBaseViewModel GetControl(string name)
    {
        _controls.TryGetValue(name, out FormControlBaseViewModel value);

        return value;
    }

    public TFormModel ToModel()
    {
        UpdateModel(_model);

        return _model;
    }

    public void SetInitialModel()
    {
        _initialModel = (TFormModel)_model.Clone();
    }

    public void ResetChanges()
    {
        _model = (TFormModel)_initialModel.Clone();
        FromModel(_model);
    }

    protected virtual void UpdateModel(TFormModel model)
    {
        throw new NotImplementedException();
    }

    protected virtual void FromModel(TFormModel model)
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