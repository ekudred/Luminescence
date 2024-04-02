using SkiaSharp;

namespace Luminescence.ViewModels;

public class ChartSeriesOptions
{
    public readonly string Name;
    public readonly SKColor Color;

    public ChartSeriesOptions(
        string name,
        SKColor? color = null
    )
    {
        Name = name;
        Color = color ?? SKColors.Chocolate;
    }
}