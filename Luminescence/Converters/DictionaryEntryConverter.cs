using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Luminescence.Form.ViewModels;

namespace Luminescence.Converters;

public class DictionaryEntryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var controls = value as Dictionary<string, FormControlBaseViewModel>;
        var controlName = parameter as string;

        if (controls == null || controlName == null)
        {
            return null;
        }

        controls.TryGetValue(controlName, out FormControlBaseViewModel control);

        return control;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}