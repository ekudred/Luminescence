﻿using System.Reactive.Subjects;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;

namespace Luminescence.Form.ViewModels;

public class FormControlBaseViewModel : ReactiveValidationObject
{
    public bool Valid { get; protected set; }

    public string Name { get; private set; }
    public string Label { get; private set; }

    public Subject<object> ValueChanges { get; } = new();

    public virtual object Value
    {
        get => refValue;
        set
        {
            this.RaiseAndSetIfChanged(ref refValue, value);

            ValueChanges.OnNext(value);
        }
    }

    public Subject<object> destroyControl;

    protected object refValue;

    protected FormControlBaseViewModel(string name)
    {
        Name = name;

        destroyControl = new();

        // this.ValidationRule(
        //     x => x.Value,
        //     this.WhenAnyValue(x => x.IsRegexSearchEnabled, x => x.SearchText),
        //     v => Valid,
        //     _ => resourceProvider.GetResourceByName(searchViewModelConfiguration.InvalidRegexResourceName)
        // );
    }

    public void Destroy()
    {
        if (destroyControl == null)
        {
            return;
        }

        destroyControl.OnNext(null!);
        destroyControl.OnCompleted();
        destroyControl = null!;
    }

    protected void SetOptions(FormControlOptions options)
    {
        Label = options.Label;
    }
}