using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class RadioControlViewModel : FormControlBaseViewModel
{
    public readonly Subject<bool> ValueChanges = new();

    public bool Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);

            if (value)
            {
                ValueChanges.OnNext(value);
            }
        }
    }

    private bool _value;

    public RadioControlViewModel(string name, RadioControlOptions options = null)
        : base(name)
    {
        SetOptions(options);
    }

    private void SetOptions(RadioControlOptions options)
    {
        if (options == null)
        {
            return;
        }

        base.SetOptions(options);
    }
}