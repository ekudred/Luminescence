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
        if (!ViewModel.SpinnerOptions.ManualInputEnabled)
        {
            args.Text = "";

            return;
        }

        string value = TextInputUtil.GetValueFromEventArgs(args);

        if (value == "")
        {
            args.Text = ViewModel.SpinnerOptions.Minimum.ToString();

            return;
        }

        if (!value.IsDecimal())
        {
            args.Text = "";

            return;
        }

        var decimalValue = value.ToDecimal();

        if (decimalValue < ViewModel.SpinnerOptions.Minimum || decimalValue > ViewModel.SpinnerOptions.Maximum)
        {
            args.Text = "";
        }
    }

    private void OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs args)
    {
       if (args.NewValue < ViewModel.SpinnerOptions.Minimum || args.NewValue > ViewModel.SpinnerOptions.Maximum)
        {
            ViewModel.Value = args.OldValue;
        }
    }
}