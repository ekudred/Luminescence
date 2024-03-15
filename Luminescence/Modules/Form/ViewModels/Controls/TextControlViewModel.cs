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
            this.RaiseAndSetIfChanged(ref refValue, value);

            ValueChanges.OnNext(value);
        }
    }

    public TextControlViewModel(string name, string defaultValue = "", TextControlOptions? options = null) : base(name)
    {
        Value = defaultValue;

        SetOptions(options ?? new());
    }

    private void SetOptions(TextControlOptions options)
    {
        Placeholder = options.Placeholder;
        RegExMask = options.RegExMask;

        base.SetOptions(options);
    }
}