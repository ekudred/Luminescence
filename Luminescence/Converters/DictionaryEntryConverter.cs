using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Luminescence.Converters;

public class DictionaryEntryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IDictionary dictionary)
        {
            return dictionary[parameter!];
        }

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}