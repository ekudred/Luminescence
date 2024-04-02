using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Luminescence.ViewModels;

public class ChartViewModel : BaseViewModel
{
    public List<ISeries> Series { get; } = new();

    public List<ISeries> ScrollbarSeries { get; } = new();


    public List<Axis> XAxes { get; } = new()
    {
        new()
        {
            TextSize = 12,
            NameTextSize = 14,
            NamePaint = new SolidColorPaint(SKColor.Parse("#333333")),
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#333333")),
            SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#d0d0d9"), 1)
        }
    };

    public List<Axis> YAxes { get; } = new()
    {
        new()
        {
            TextSize = 12,
            NameTextSize = 14,
            NamePaint = new SolidColorPaint(SKColor.Parse("#333333")),
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#333333")),
            SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#d0d0d9"), 1)
        }
    };

    public Axis[] ScrollbarXAxes { get; } =
    {
        new()
        {
            IsVisible = false,
            MinStep = TimeSpan.FromMilliseconds(1).Ticks,
            MinZoomDelta = TimeSpan.FromMilliseconds(1).Ticks
        }
    };

    public Axis[] ScrollbarYAxes { get; } =
    {
        new()
        {
            IsVisible = false,
            MinStep = TimeSpan.FromMilliseconds(1).Ticks,
            MinZoomDelta = TimeSpan.FromMilliseconds(1).Ticks
        }
    };

    public Margin Margin { get; } = new(100, Margin.Auto, 50, Margin.Auto);

    public RectangularSection[] Thumbs { get; } =
    {
        new()
        {
            Fill = new SolidColorPaint(SKColor.Parse("#d0d0d9"))
        }
    };

    public object Sync { get; } = new();

    public ICommand ChartUpdatedCommand { get; }
    public ICommand PointerDownCommand { get; }
    public ICommand PointerMoveCommand { get; }
    public ICommand PointerUpCommand { get; }

    private readonly int _visiblePoints;
    private bool _isDown;

    public ChartViewModel(ChartAxisOptions xAxisOptions, ChartAxisOptions yAxisOptions)
    {
        var xAxis = XAxes[0];
        xAxis.Name = xAxisOptions.Name;
        xAxis.MinLimit = xAxisOptions.MinLimit;
        xAxis.MaxLimit = xAxisOptions.MaxLimit;
        xAxis.MinStep = xAxisOptions.MinStep;
        xAxis.MinZoomDelta = xAxis.MinStep;

        var yAxis = YAxes[0];
        yAxis.Name = yAxisOptions.Name;
        yAxis.MinLimit = yAxisOptions.MinLimit;
        yAxis.MaxLimit = yAxisOptions.MaxLimit;
        yAxis.MinStep = yAxisOptions.MinStep;
        yAxis.MinZoomDelta = yAxis.MinStep;

        ChartUpdatedCommand = ReactiveCommand.Create<ChartCommandArgs>(ChartUpdated);
        PointerDownCommand = ReactiveCommand.Create<PointerCommandArgs>(PointerDown);
        PointerMoveCommand = ReactiveCommand.Create<PointerCommandArgs>(PointerMove);
        PointerUpCommand = ReactiveCommand.Create<PointerCommandArgs>(PointerUp);
    }

    public void AddPoint(string seriesName, double xValue, double yValue)
    {
        var series = Series.Find(series => series.Name == seriesName);
        var scrollbarSeries = ScrollbarSeries.Find(scrollbarSeries => scrollbarSeries.Name == seriesName);

        if (series == null || scrollbarSeries == null)
        {
            throw new Exception($"Series \"{seriesName}\" not found");
        }

        lock (Sync)
        {
            var seriesValues = (ObservableCollection<ObservablePoint>)series.Values!;
            var scrollbarSeriesValues = (ObservableCollection<ObservablePoint>)scrollbarSeries.Values!;

            seriesValues.Add(new ObservablePoint(xValue, yValue));
            scrollbarSeriesValues.Add(new ObservablePoint(xValue, yValue));

            // if (seriesValues.Count > _visiblePoints)
            // {
            //     seriesValues.RemoveAt(0);
            //     scrollbarSeriesValues.RemoveAt(0);
            // }

            // if (seriesValues.Count > 2 && seriesValues[^2].X <= Thumbs[0].Xj)
            // {
            //     XAxes[0].MinLimit = Thumbs[0].Xi + (seriesValues[^1].X - seriesValues[^2].X);
            //     XAxes[0].MaxLimit = seriesValues[^1].X;
            // }
        }
    }

    public void AddSeries(ChartSeriesOptions seriesOptions)
    {
        Series.Add(new LineSeries<ObservablePoint>
        {
            Name = seriesOptions.Name,
            Values = new ObservableCollection<ObservablePoint>(),
            Stroke = new SolidColorPaint(seriesOptions.Color, 1),
            Fill = null,
            GeometrySize = 1,
            LineSmoothness = 0.5,
            GeometryFill = null,
            GeometryStroke = null,
            IsHoverable = false
        });
        ScrollbarSeries.Add(new LineSeries<ObservablePoint>
        {
            Name = seriesOptions.Name,
            Values = new ObservableCollection<ObservablePoint>(),
            Stroke = new SolidColorPaint(seriesOptions.Color, 1),
            Fill = null,
            GeometrySize = 1,
            LineSmoothness = 0.5,
            GeometryFill = null,
            GeometryStroke = null,
            IsHoverable = false
        });
    }

    private void ChartUpdated(ChartCommandArgs args)
    {
        RectangularSection thumb = Thumbs[0];
        ICartesianAxis xAxis = ((ICartesianChartView<SkiaSharpDrawingContext>)args.Chart).XAxes.First();
        thumb.Xi = xAxis.MinLimit;
        thumb.Xj = xAxis.MaxLimit;
    }

    private void PointerDown(PointerCommandArgs args)
    {
        _isDown = true;
    }

    private void PointerMove(PointerCommandArgs args)
    {
        if (!_isDown)
        {
            return;
        }

        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
        var positionInData = chart.ScalePixelsToData(args.PointerPosition);

        RectangularSection thumb = Thumbs[0];
        double? currentRange = thumb.Xj - thumb.Xi;

        thumb.Xi = positionInData.X - currentRange / 2;
        thumb.Xj = positionInData.X + currentRange / 2;

        Axis xAxis = XAxes[0];
        xAxis.MinLimit = thumb.Xi;
        xAxis.MaxLimit = thumb.Xj;
    }

    private void PointerUp(PointerCommandArgs args)
    {
        _isDown = false;
    }
}