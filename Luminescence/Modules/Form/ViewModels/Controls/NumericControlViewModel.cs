using Luminescence.Shared.Utils;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class NumericControlViewModel : FormControlBaseViewModel
{
    public string Placeholder { get; private set; }

    public NumericControlSpinnerOptions SpinnerOptions { get; private set; }

    public override object Value
    {
        get => refValue;
        set
        {
            if (value is double doubleValue)
            {
                value = doubleValue.ToDecimal()!;
            }
            if (value is string stringValue)
            {
                value = stringValue.ToDecimal()!;
            }

            this.RaiseAndSetIfChanged(ref refValue, value);

            ValueChanges.OnNext(value);
        }
    }

    public NumericControlViewModel(string name, decimal defaultValue = 0, NumericControlOptions? options = null)
        : base(name)
    {
        Value = defaultValue;

        SetOptions(options ?? new());
    }

    private void SetOptions(NumericControlOptions options)
    {
        Placeholder = options.Placeholder;
        SpinnerOptions = options.Spinner;

        base.SetOptions(options);
    }
}