﻿using Avalonia;
using Avalonia.Input;
using Luminescence.Dialog;
using Luminescence.ViewModels;

namespace Luminescence.Views;

public partial class SettingsDialog : DialogWindow<SettingsDialogViewModel>
{
    public SettingsDialog()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    protected override void Initialize()
    {
        base.Initialize();

        ViewModel.UseConfirm(this);
    }

    protected override void OnKeyUp(KeyEventArgs args)
    {
        if (args.Key == Key.Enter)
        {
            if (ViewModel.FormChanged)
            {
                ViewModel.ApplyCommand.Execute(null);
            }
        }
    }
}