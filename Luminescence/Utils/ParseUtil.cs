using System;
using System.Globalization;

namespace Luminescence.Utils;

public static class ParseUtil
{
    public static int ToInt(this string value)
    {
        int.TryParse(value, out int result);

        return result;
    }

    public static double? ToDouble(this string value)
    {
        double.TryParse(value, out double result);

        return result;
    }

    public static double? ToDouble(this decimal value)
    {
        return decimal.ToDouble(value);
    }

    public static decimal? ToDecimal(this string value)
    {
        decimal.TryParse(value, out decimal result);

        return result;
    }

    public static decimal? ToDecimal(this double value)
    {
        return Convert.ToDecimal(value);
    }

    public static string? FromObject(this object value)
    {
        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }
}