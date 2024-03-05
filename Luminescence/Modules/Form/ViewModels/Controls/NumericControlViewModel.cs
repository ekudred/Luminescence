using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class NumericControlViewModel : FormControlBaseViewModel
{
    public NumericControlSpinnerOptions SpinnerOptions { get; private set; }

    public NumericControlViewModel(string name, double defaultValue = 0, NumericControlOptions? options = null)
        : base(name)
    {
        Value = defaultValue;

        SetOptions(options ?? new());
    }

    private void SetOptions(NumericControlOptions options)
    {
        SpinnerOptions = options.Spinner;

        base.SetOptions(options);
    }
}