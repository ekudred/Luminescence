using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Luminescence.Chart;

public class ChartModel : PlotModel
{
    // public ChartModel(Axis AbscissaAxis, Axis OrdinateAxis, Series Series)
    public ChartModel(string abscissaTitle, string ordinateTitle)
    {
        double min = 0;
        double max = 1e8;

        Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left, Minimum = min, Maximum = max, Title = abscissaTitle,
                AxisTitleDistance = 24
            }
        );
        Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom, Title = ordinateTitle,
                AxisTitleDistance = 24
            }
        );
        Series.Add(new FunctionSeries
            (x => Math.Sin(x * Math.PI * 4) * Math.Sin(x * Math.PI * 4) * Math.Sqrt(x) * max, 0, 1, 1000)
            { Color = OxyColor.Parse("#22b937") }
        );
    }
}