using Luminescence.Dialog;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ErrorDialogViewModel : DialogBaseViewModel
{
    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    private string _text;

    public override void Initialize(object o)
    {
        var data = o as ErrorDialogParam;

        Text = data.Text;
    }
}