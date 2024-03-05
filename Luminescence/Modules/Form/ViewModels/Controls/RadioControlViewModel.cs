using System;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class RadioControlViewModel : FormControlBaseViewModel
{
    public override object Value
    {
        get => refValue;
        set
        {
            this.RaiseAndSetIfChanged(ref refValue, value);

            if ((bool)value)
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

    private string _groupId = Guid.NewGuid().ToString();

    public RadioControlViewModel(string name, RadioControlOptions? options = null)
        : base(name)
    {
        SetOptions(options ?? new());
    }

    private void SetOptions(RadioControlOptions options)
    {
        base.SetOptions(options);
    }
}