using System.Reactive.Subjects;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class FormControlBaseViewModel : BaseViewModel
{
    public readonly string Name;
    public string Label { get; set; }

    public Subject<string> ValueChanges = new Subject<string>();
    public string Value
    {
        get => _value;
        set
        {
            ValueChanges.OnNext(value);
            
            this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    private string _value = "";

    public FormControlBaseViewModel(string name)
    {
        Name = name == null ? "" : name;
    }

    protected void SetOptions(FormControlOptions options)
    {
        if (options == null)
        {
            return;
        }

        Label = options.Label == null ? "" : options.Label;
    }
}