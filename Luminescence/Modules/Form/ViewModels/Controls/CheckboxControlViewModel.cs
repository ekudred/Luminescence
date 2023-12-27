using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class CheckboxControlViewModel : FormControlBaseViewModel
{
    public Subject<bool> ValueChanges = new Subject<bool>();

    public bool Value
    {
        get => _value;
        set
        {
            ValueChanges.OnNext(value);

            this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    private bool _value = false;

    public CheckboxControlViewModel(string name, bool defaultValue = false, CheckboxControlOptions options = null)
        : base(name)
    {
        Value = defaultValue == null ? false : defaultValue;

        SetOptions(options);
    }

    private void SetOptions(CheckboxControlOptions options)
    {
        if (options == null)
        {
            return;
        }

        base.SetOptions(options);
    }
}