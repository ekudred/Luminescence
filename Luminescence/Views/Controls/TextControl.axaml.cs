using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Luminescence.Form.ViewModels;

namespace Luminescence.Views;

public partial class TextControl : UserControl
{
    public TextControlViewModel ViewModel => (TextControlViewModel)DataContext!;

    public TextControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        AddHandler(TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
    }

    private void OnTextInput(object? sender, TextInputEventArgs args)
    {
        TextBox control = (TextBox)args.Source!;
        string text = control.Text ?? string.Empty;
        string value = text.Substring(0, control.CaretIndex) + args.Text + text.Substring(control.CaretIndex);

        if (ViewModel.RegExMask.IsMatch(value))
        {
            return;
        }

        args.Text = "";
    }
}