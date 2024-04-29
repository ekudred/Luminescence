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

    public static int ToInt(this double value)
    {
        return Convert.ToInt32(value);
    }

    public static int ToInt(this decimal value)
    {
        return Convert.ToInt32(value);
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

    public static double? ToDouble(this float value)
    {
        return Convert.ToDouble(value);
    }

    public static double? ToDouble(this uint value)
    {
        return Convert.ToDouble(value);
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
}