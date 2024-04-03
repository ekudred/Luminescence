using System.Reactive;
using System.Reactive.Subjects;
using Luminescence.Dialog;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ConfirmationDialogViewModel : DialogBaseViewModel
{
    public readonly Subject<bool> OnConfirm = new();

    public ReactiveCommand<bool, Unit> ConfirmCommand { get; }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    private string _text;

    public ConfirmationDialogViewModel()
    {
        ConfirmCommand = ReactiveCommand.Create<bool>(OnConfirm.OnNext);
    }

    public override void Initialize(object o)
    {
        var data = o as ConfirmationDialogData;

        Text = data.Text;
    }
}