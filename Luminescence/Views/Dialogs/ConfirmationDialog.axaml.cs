using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Luminescence.Dialog;
using Luminescence.ViewModels;

namespace Luminescence.Views;

public partial class ConfirmationDialog : DialogWindow<ConfirmationDialogViewModel>
{
    public ConfirmationDialog()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    protected override void OnOpened(EventArgs args)
    {
        this.FindControl<Button>("ButtonConfirmFocus").Focus();

        base.OnOpened(args);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key == Key.Escape)
        {
            Close();
        }
    }
}