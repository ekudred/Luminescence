using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Luminescence.Form.ViewModels;
using Luminescence.Shared.Utils;

namespace Luminescence.Views;

public partial class TextControl : UserControl
{
    public TextControlViewModel ViewModel => (TextControlViewModel)DataContext!;

    public TextControl()
    {
        InitializeComponent();

        AddHandler(TextBox.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
    }

    private void OnTextInput(object? sender, TextInputEventArgs args)
    {
        string value = TextInputUtil.GetValueFromEventArgs(args);

        if (ViewModel.RegExMask.IsMatch(value))
        {
            return;
        }

        args.Text = "";
    }
}