using Avalonia;
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
}