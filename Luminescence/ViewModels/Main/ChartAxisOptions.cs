using System;

namespace Luminescence.ViewModels;

public class ChartAxisOptions
{
    public readonly string Name;
    public readonly double? MinLimit;
    public readonly double? MaxLimit;
    public readonly double MinStep;

    public ChartAxisOptions(
        string name,
        double? minLimit = null,
        double? maxLimit = null,
        double? minStep = null
    )
    {
        Name = name;
        MinLimit = minLimit;
        MaxLimit = maxLimit;
        MinStep = minStep ?? TimeSpan.FromMilliseconds(1).Ticks;
    }
}