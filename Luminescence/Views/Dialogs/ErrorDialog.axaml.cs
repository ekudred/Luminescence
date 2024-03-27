using Avalonia;
using Avalonia.Input;
using Luminescence.Dialog;
using Luminescence.ViewModels;

namespace Luminescence.Views;

public partial class ErrorDialog : DialogWindow<ErrorDialogViewModel>
{
    public ErrorDialog()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key == Key.Escape)
        {
            Close();
        }
    }
}