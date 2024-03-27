using System;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Luminescence.Form.ViewModels;

namespace Luminescence.Views;

public partial class NumericControl : UserControl
{
    public NumericControlViewModel ViewModel => (NumericControlViewModel)DataContext!;

    public NumericControl()
    {
        InitializeComponent();

        AddHandler(TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
    }

    private void OnTextInput(object? sender, TextInputEventArgs args)
    {
        // NumericUpDown control = (NumericUpDown)args.Source!;
        // string text = control.Text ?? string.Empty;
        // string value = text.Substring(0, control.CaretIndex) + args.Text + text.Substring(control.CaretIndex);
        //
        // if (new Regex(@"^\d+$").IsMatch(value))
        // {
        //     return;
        // }
        //
        // args.Text = "";
    }
}