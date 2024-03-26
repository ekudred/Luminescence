using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Axis = LiveChartsCore.SkiaSharpView.Axis;

namespace Luminescence.ViewModels;

public class ChartViewModel : BaseViewModel
{
    public List<LineSeries<ObservablePoint>> Series { get; }

    public List<Axis> XAxes { get; }

    public List<Axis> YAxes { get; }

    public ChartViewModel(string xAxisName, string yAxisName)
    {
        Series = new()
        {
            new LineSeries<ObservablePoint>
            {
                Name = "Name",
                Values = new List<ObservablePoint>()
            }
        };
        XAxes = new()
        {
            new Axis { Name = xAxisName }
        };
        YAxes = new()
        {
            new Axis { Name = yAxisName }
        };
    }

    public void AddPoint(string seriesName, double xValue, double yValue)
    {
        LineSeries<ObservablePoint> series = Series.Find(series => series.Name == seriesName);

        if (series == null)
        {
            throw new Exception($"Series \"{seriesName}\" not found");
        }

        ((List<ObservablePoint>)series.Values).Add(new ObservablePoint(xValue, yValue));
    }
}