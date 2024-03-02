using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class TextControlViewModel : FormControlBaseViewModel
{
    public new readonly Subject<string> ValueChanges = new();

    public new string Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);

            ValueChanges.OnNext(value);
        }
    }

    private string _value;

    public TextControlViewModel(string name, string defaultValue = "", TextControlOptions options = null)
        : base(name)
    {
        Value = defaultValue == null ? "" : defaultValue;

        SetOptions(options);
    }

    private void SetOptions(TextControlOptions options)
    {
        if (options == null)
        {
            return;
        }

        base.SetOptions(options);
    }
}