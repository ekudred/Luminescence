using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Luminescence.Converters;

/// DataContext="{Binding DictionaryEntries, Converter={StaticResource DictionaryEntryConverter}, ConverterParameter={x:Static s:DictionaryEntryKey}}"
public class DictionaryEntryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IDictionary dictionary)
        {
            throw new Exception($"\"{value}\" is not \"IDictionary\"");
        }

        if (value == null || parameter == null)
        {
            throw new Exception($"Value \"{value}\" or parameter \"{parameter}\" is null");
        }

        if (!dictionary.Contains(parameter))
        {
            throw new Exception($"The dictionary \"{dictionary}\" does not contain a key \"{parameter}\"");
        }

        return dictionary[parameter];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}