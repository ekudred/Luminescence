using System;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class RadioControlViewModel : FormControlBaseViewModel
{
    public new readonly Subject<bool> ValueChanges = new();

    public new bool Value
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

    public string GroupId
    {
        get => _groupId;
        set => this.RaiseAndSetIfChanged(ref _groupId, value);
    }

    private bool _value;
    private string _groupId = Guid.NewGuid().ToString();

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