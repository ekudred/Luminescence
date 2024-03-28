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
    public Dictionary<string, FormControlBaseViewModel> Controls { get; } = new();
    public readonly Subject<bool> FormChanged = new();

    public bool Initialized = false;
    public bool Loading = false;

    public Subject<object> destroyForm;

    private readonly Subject<object> _onChanges = new();

    private TFormModel _model;
    private TFormModel _initialModel;

    public void Initialize()
    {
        destroyForm = new();

        GetControls(new())
            .ForEach(control => Controls.Add(control.Name, control));

        _model = Activator.CreateInstance<TFormModel>();
        _initialModel = (TFormModel)_model.Clone();

        UpdateModel();
        UpdateInitialModel();

        Controls
            .Select(control => control.Value.ValueChanges).Merge()
            .TakeUntil(destroyForm)
            .Subscribe(_ => { CheckChanges(); });

        _onChanges
            .TakeUntil(destroyForm)
            .Subscribe(_ => { FormChanged.OnNext(!ToModel().Equals(_initialModel)); });
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

    public void Apply()
    {
        UpdateModel();
        UpdateInitialModel();
        CheckChanges();
    }

    public void Cancel()
    {
        ResetModel();
        FromModel(_initialModel);
        CheckChanges();
    }

    public void CheckChanges()
    {
        _onChanges.OnNext(default);
    }

    public FormControlBaseViewModel? GetControl(string controlName)
    {
        FormControlBaseViewModel? control = null;

        try
        {
            Controls.TryGetValue(controlName, out control);

            if (control == null)
            {
                throw new Exception($"Control \"{controlName}\" not found");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return control;
    }

    public void SetControlValue<T>(string controlName, T value)
    {
        try
        {
            var control = GetControl(controlName);

            if (control == null)
            {
                return;
            }

            control.Value = value;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    public T? GetControlValue<T>(string controlName)
    {
        T? value = default;

        try
        {
            var control = GetControl(controlName);

            if (control == null)
            {
                return value;
            }

            if (control.Value.GetType() != typeof(T))
            {
                throw new Exception(
                    $"The Value \"{control.Value}\" of the Control \"{control.Name}\" with type \"{control.Value.GetType()}\" does not match the specified type \"{typeof(T)}\"");
            }

            value = (T?)control.Value;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return value;
    }

    public TFormModel ToModel()
    {
        ChangeModel(_model);

        return _model;
    }

    public void UpdateModel()
    {
        ChangeModel(_model);
    }

    public void ResetModel()
    {
        _model = (TFormModel)_initialModel.Clone();
    }

    public void UpdateInitialModel()
    {
        ChangeModel(_initialModel);
    }

    public virtual void FromModel(TFormModel model)
    {
        throw new NotImplementedException();
    }

    protected virtual void ChangeModel(TFormModel model)
    {
        throw new NotImplementedException();
    }

    protected virtual List<FormControlBaseViewModel> GetControls(List<FormControlBaseViewModel> list)
    {
        throw new NotImplementedException();
    }
}