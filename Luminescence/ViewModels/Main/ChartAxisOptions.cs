using System;

namespace Luminescence.ViewModels;

public class ChartAxisOptions
{
    public readonly string Name;
    public readonly double? MinLimit;
    public readonly double? MaxLimit;

    public ChartAxisOptions(
        string name,
        double? minLimit = null,
        double? maxLimit = null
    )
    {
        Name = name;
        MinLimit = minLimit;
        MaxLimit = maxLimit;
    }
}