namespace Luminescence.Shared.Utils;

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

    public static uint ToUInt(this decimal value)
    {
        return Convert.ToUInt32(value);
    }

    public static double? ToDouble(this string value)
    {
        double.TryParse(value, out double result);

        return result;
    }

    public static double? ToDouble(this int value)
    {
        return Convert.ToDouble(value);
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
        var parse = decimal.TryParse(value, out decimal result);

        return parse ? result : 0;
    }

    public static bool IsDecimal(this string value)
    {
        return Decimal.TryParse(value, out _);
    }

    public static decimal? ToDecimal(this uint value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal? ToDecimal(this int value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal? ToDecimal(this double value)
    {
        return Convert.ToDecimal(value);
    }

    public static decimal? ToDecimal(this float value)
    {
        return Convert.ToDecimal(value);
    }
}