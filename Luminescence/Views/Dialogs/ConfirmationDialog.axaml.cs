using System;
using Avalonia;
using Avalonia.Controls;
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
}