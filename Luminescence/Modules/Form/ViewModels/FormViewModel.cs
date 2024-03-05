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

    private readonly TFormModel _model = Activator.CreateInstance<TFormModel>();
    private TFormModel _initialModel;

    private Dictionary<string, FormControlBaseViewModel> _controls = new();

    public void Initialize()
    {
        destroyForm = new();

        GetControls(new())
            .ForEach(control => _controls.Add(control.Name, control));

        UpdateModel(_model);
        UpdateInitialModel();

        Controls
            .Select(control => control.Value.ValueChanges)
            .Merge()
            .TakeUntil(destroyForm)
            .Subscribe(_ =>
            {
                var a = ToModel();
                var b = _initialModel;
                FormChanged.OnNext(!Equals(a, b));
            });

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

        destroyForm.OnNext(0);
        destroyForm.OnCompleted();
        destroyForm = null;
    }

    public TFormModel ToModel()
    {
        UpdateModel(_model);

        return _model;
    }

    public FormControlBaseViewModel GetControl(string name)
    {
        _controls.TryGetValue(name, out FormControlBaseViewModel value);

        return value;
    }

    private void UpdateInitialModel()
    {
        _initialModel = (TFormModel)_model.Clone();
    }

    protected virtual void UpdateModel(TFormModel model)
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