using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class TextControlViewModel : FormControlBaseViewModel
{
    public override object Value
    {
        get => refValue;
        set
        {
            this.RaiseAndSetIfChanged(ref refValue, value);

            ValueChanges.OnNext(value);
        }
    }

    public TextControlViewModel(string name, string defaultValue = "", TextControlOptions? options = null)
        :
        base(name)
    {
        Value = defaultValue;

        SetOptions(options ?? new());
    }

    private void SetOptions(TextControlOptions options)
    {
        base.SetOptions(options);
    }
}