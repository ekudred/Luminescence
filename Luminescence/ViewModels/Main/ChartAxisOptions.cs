namespace Luminescence.ViewModels;

public class ChartAxisOptions
{
    public readonly string Name;
    public readonly string MeasureName;
    public readonly double? MinLimit;
    public readonly double? MaxLimit;

    public ChartAxisOptions(
        string name,
        string measureName,
        double? minLimit = null,
        double? maxLimit = null
    )
    {
        Name = name;
        MeasureName = measureName;
        MinLimit = minLimit;
        MaxLimit = maxLimit;
    }
}