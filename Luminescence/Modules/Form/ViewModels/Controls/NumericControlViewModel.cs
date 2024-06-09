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

            if (value is decimal decimalValue)
            {
                this.RaiseAndSetIfChanged(ref refValue, decimalValue);
                ValueChanges.OnNext(value);
            }
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

        var value = (decimal)Value;

        if (value < SpinnerOptions.Minimum)
        {
            Value = SpinnerOptions.Minimum;
        }

        if (value > SpinnerOptions.Maximum)
        {
            Value = SpinnerOptions.Maximum;
        }

        base.SetOptions(options);
    }
}