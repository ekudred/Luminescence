using System.Reactive.Subjects;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class FormControlBaseViewModel : BaseViewModel
{
    public string Name { get; private set; }
    public string Label { get; private set; }

    public Subject<object> ValueChanges { get; } = new();

    public virtual object Value
    {
        get => refValue;
        set
        {
            this.RaiseAndSetIfChanged(ref refValue, value);

            ValueChanges.OnNext(value);
        }
    }

    public Subject<object> destroyControl;

    protected object refValue;

    protected FormControlBaseViewModel(string name)
    {
        Name = name;

        destroyControl = new();
    }

    public void Destroy()
    {
        if (destroyControl == null)
        {
            return;
        }

        destroyControl.OnNext(0);
        destroyControl.OnCompleted();
        destroyControl = null;
    }

    protected void SetOptions(FormControlOptions options)
    {
        Label = options.Label;
    }
}