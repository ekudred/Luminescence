using System;
using System.Reactive.Subjects;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class FormControlBaseViewModel : BaseViewModel
{
    public readonly string Name;
    public string Label { get; set; }

    public readonly Subject<object> ValueChanges = new();

    public object Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);

            ValueChanges.OnNext(value);
        }
    }

    private object _value;

    protected FormControlBaseViewModel(string name)
    {
        Name = name;
    }

    protected void SetOptions(FormControlOptions options)
    {
        if (options == null)
        {
            return;
        }

        Label = options.Label ?? "";
    }
}