using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using SkiaSharp;

namespace Luminescence.ViewModels;

public class ChartCustomLegend : IChartLegend<SkiaSharpDrawingContext>
{
    private static readonly int s_zIndex = 10050;
    private readonly StackPanel<RoundedRectangleGeometry, SkiaSharpDrawingContext> _stackPanel = new();

    public void Draw(Chart<SkiaSharpDrawingContext> chart)
    {
        // _stackPanel.X = (float)(chart.ControlSize.Width * 0.5 - chart.ControlSize.Width * 0.5);
        // _stackPanel.X = chart.View.;
        //
        // if (chart.View.Title != null)
        // {
        //     chart.View.Title!.ClippingMode = ClipMode.None;
        //     LvcSize lvcSize = chart.View.Title.Measure(chart);
        //     _stackPanel.Y = lvcSize.Height;
        // }

        chart.AddVisual(_stackPanel);
        if (chart.LegendPosition == LegendPosition.Hidden)
        {
            chart.RemoveVisual(_stackPanel);
        }
    }

    public LvcSize Measure(Chart<SkiaSharpDrawingContext> chart)
    {
        _stackPanel.Orientation = ContainerOrientation.Horizontal;
        _stackPanel.HorizontalAlignment = Align.End;
        _stackPanel.MaxWidth = chart.ControlSize.Width;
        _stackPanel.MaxHeight = chart.ControlSize.Height;

        foreach (var visual in _stackPanel.Children.ToArray())
        {
            _ = _stackPanel.Children.Remove(visual);
            chart.RemoveVisual(visual);
        }

        foreach (var chartSeries in chart.Series.Where(x => x.IsVisibleAtLegend))
        {
            var series = (LineSeries<ObservablePoint>)chartSeries;

            var panel = new StackPanel<RectangleGeometry, SkiaSharpDrawingContext>
            {
                Padding = new Padding(12, 6),
                VerticalAlignment = Align.Middle,
                HorizontalAlignment = Align.Middle,
                Children =
                {
                    series.GetMiniaturesSketch().AsDrawnControl(s_zIndex),
                    new LabelVisual
                    {
                        Text = series.Name ?? string.Empty,
                        Paint = new SolidColorPaint(new SKColor(30, 20, 30))
                        {
                            SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal),
                            ZIndex = s_zIndex + 1
                        },
                        TextSize = 12,
                        Padding = new Padding(8, 0, 0, 0),
                        VerticalAlignment = Align.Start,
                        HorizontalAlignment = Align.Start
                    }
                }
            };

            panel.PointerDown += GetPointerDownHandler(series);
            _stackPanel.Children.Add(panel);
        }

        return _stackPanel.Measure(chart);
    }

    private static VisualElementHandler<SkiaSharpDrawingContext> GetPointerDownHandler(
        IChartSeries<SkiaSharpDrawingContext> series)
    {
        return (_, _) => { series.IsVisible = !series.IsVisible; };
    }
}