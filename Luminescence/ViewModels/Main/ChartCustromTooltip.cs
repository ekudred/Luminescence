using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using SkiaSharp;

namespace Luminescence.ViewModels;

public class ChartCustomTooltip : IChartTooltip<SkiaSharpDrawingContext>
{
    private StackPanel<RoundedRectangleGeometry, SkiaSharpDrawingContext>? _stackPanel;
    private static readonly int s_zIndex = 10100;

    public void Show(IEnumerable<ChartPoint> foundPoints, Chart<SkiaSharpDrawingContext> chart)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)chart;
        var xAxesMeasureName = cartesianChart.XAxes[0].Name.Split(",")[1].Trim();
        var yAxesMeasureName = cartesianChart.YAxes[0].Name.Split(",")[1].Trim();

        if (_stackPanel is null)
        {
            _stackPanel = new StackPanel<RoundedRectangleGeometry, SkiaSharpDrawingContext>
            {
                Padding = new Padding(8, 4),
                Orientation = ContainerOrientation.Vertical,
                HorizontalAlignment = Align.Start,
                VerticalAlignment = Align.Middle,
                BackgroundPaint = new SolidColorPaint(SKColor.Parse("#f7f7f7")) { ZIndex = s_zIndex }
            };
        }

        foreach (var child in _stackPanel.Children.ToArray())
        {
            _ = _stackPanel.Children.Remove(child);
            chart.RemoveVisual(child);
        }

        foreach (var point in foundPoints)
        {
            var series = (IChartSeries<SkiaSharpDrawingContext>)point.Context.Series;
            var relativePanel = series.GetMiniaturesSketch().AsDrawnControl(s_zIndex);

            var label = new LabelVisual
            {
                // Text = point.Coordinate.PrimaryValue.ToString("C2"),
                Text =
                    $"{point.Coordinate.SecondaryValue} {xAxesMeasureName}, {point.Coordinate.PrimaryValue} {yAxesMeasureName}",
                Paint = new SolidColorPaint(new SKColor(30, 20, 30))
                {
                    SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal),
                    ZIndex = s_zIndex + 1
                },
                TextSize = 12,
                Padding = new Padding(8, 0, 0, 0),
                VerticalAlignment = Align.Start,
                HorizontalAlignment = Align.Start
            };

            var sp = new StackPanel<RoundedRectangleGeometry, SkiaSharpDrawingContext>
            {
                Padding = new Padding(0, 4),
                VerticalAlignment = Align.Middle,
                HorizontalAlignment = Align.Middle,
                Children = { relativePanel, label }
            };

            _stackPanel?.Children.Add(sp);
        }

        var size = _stackPanel.Measure(chart);

        var location = foundPoints.GetTooltipLocation(size, chart);

        _stackPanel.X = location.X;
        _stackPanel.Y = location.Y;

        chart.AddVisual(_stackPanel);
    }

    public void Hide(Chart<SkiaSharpDrawingContext> chart)
    {
        if (chart is null || _stackPanel is null) return;
        chart.RemoveVisual(_stackPanel);
    }
}