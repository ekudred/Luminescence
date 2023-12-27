using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class RadioControlViewModel : FormControlBaseViewModel
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
    public double Group { get; set; }

    public RadioControlViewModel(string name, bool defaultValue = false, RadioControlOptions options = null)
        : base(name)
    {
        Value = defaultValue == null ? false : defaultValue;

        SetOptions(options);
    }

    private void SetOptions(RadioControlOptions options)
    {
        if (options == null)
        {
            return;
        }

        Group = options.Group == null ? 0 : options.Group;

        base.SetOptions(options);
    }
}