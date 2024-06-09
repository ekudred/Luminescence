using System;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Luminescence.Form.ViewModels;
using Luminescence.Shared.Utils;

namespace Luminescence.Views;

public partial class NumericControl : UserControl
{
    public NumericControlViewModel ViewModel => (NumericControlViewModel)DataContext!;

    public NumericControl()
    {
        InitializeComponent();

        AddHandler(NumericUpDown.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
        AddHandler(NumericUpDown.ValueChangedEvent, OnValueChanged, RoutingStrategies.Bubble);
    }

    private void OnTextInput(object? sender, TextInputEventArgs args)
    {
        string value = TextInputUtil.GetValueFromEventArgs(args);

        if (value == "")
        {
            args.Text = ViewModel.SpinnerOptions.Minimum.ToString();

            return;
        }

        if (!new Regex(@"^\d+$").IsMatch(value))
        {
            return;
        }

        if (value.ToDecimal() < ViewModel.SpinnerOptions.Minimum)
        {
            args.Text = "";
            ViewModel.Value = ViewModel.SpinnerOptions.Minimum;
        }

        if (value.ToDecimal() > ViewModel.SpinnerOptions.Maximum)
        {
            args.Text = "";
            ViewModel.Value = ViewModel.SpinnerOptions.Maximum;
        }
    }

    private void OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs args)
    {
        if (args.NewValue < ViewModel.SpinnerOptions.Minimum)
        {
            ViewModel.Value = ViewModel.SpinnerOptions.Minimum;
        }

        if (args.NewValue > ViewModel.SpinnerOptions.Maximum)
        {
            ViewModel.Value = ViewModel.SpinnerOptions.Maximum;
        }
    }
}