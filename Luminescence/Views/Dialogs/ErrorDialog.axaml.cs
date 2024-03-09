using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key == Key.Escape)
        {
            Close();
        }
    }
}