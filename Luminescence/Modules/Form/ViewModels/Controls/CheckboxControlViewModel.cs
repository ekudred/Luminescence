using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class CheckboxControlViewModel : FormControlBaseViewModel
{
    public new readonly Subject<bool> ValueChanges = new();

    public new bool Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);

            ValueChanges.OnNext(value);
        }
    }

    private bool _value;

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