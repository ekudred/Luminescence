using System.Text.RegularExpressions;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class TextControlViewModel : FormControlBaseViewModel
{
    public string Placeholder { get; private set; }

    public Regex RegExMask { get; private set; }

    public override object Value
    {
        get => refValue;
        set
        {
            SetRefValue(value);

            ValueChanges.OnNext(value);
        }
    }

    public TextControlViewModel(string name, string defaultValue = "", TextControlOptions? options = null) : base(name)
    {
        SetRefValue(defaultValue);

        SetOptions(options ?? new());
    }

    private void SetOptions(TextControlOptions options)
    {
        Placeholder = options.Placeholder;
        RegExMask = options.RegExMask;

        base.SetOptions(options);
    }

    private void SetRefValue(object value)
    {
        this.RaiseAndSetIfChanged(ref refValue, value);
    }
}