using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore.Defaults;
using Luminescence.ViewModels;
using SkiaSharp;

namespace Luminescence.Services;

public class ExpChartsModel
{
    public static readonly string Tal0 = "Tal0";
    public static readonly string Tal1 = "Tal1";
    public static readonly string Tl = "Tl";
    public static readonly string Osl = "Osl";
    public static readonly string Led = "Led";

    public static readonly string CurrentSeriesName = "Текущий";
    public static readonly string OpSeriesName = "Опорный";

    public readonly Dictionary<string, ChartViewModel> ChartViewModels = new();

    public void Initialize()
    {
        ChartViewModel tal0Chart = new(Tal0, new("Время", "сек"), new("Температура", "°C"));
        tal0Chart.AddSeries(new(CurrentSeriesName));
        tal0Chart.AddSeries(new(OpSeriesName, SKColors.Gold));
        ChartViewModels.Add(tal0Chart.Name, tal0Chart);

        ChartViewModel tal1Chart = new(Tal1, new("Время", "сек"), new("Интенсивность", "УЕ"));
        tal1Chart.AddSeries(new(CurrentSeriesName));
        ChartViewModels.Add(tal1Chart.Name, tal1Chart);

        ChartViewModel tlChart = new(Tl, new("Температура", "°C"), new("Интенсивность", "УЕ"));
        tlChart.AddSeries(new(CurrentSeriesName));
        ChartViewModels.Add(tlChart.Name, tlChart);

        ChartViewModel oslChart = new(Osl, new("Ток светодиода", "мА"), new("Интенсивность", "УЕ"));
        oslChart.AddSeries(new(CurrentSeriesName));
        ChartViewModels.Add(oslChart.Name, oslChart);

        ChartViewModel ledChart = new(Led, new("Время", "сек"), new("Ток светодиода", "мА"));
        ledChart.AddSeries(new(CurrentSeriesName));
        ledChart.AddSeries(new(OpSeriesName, SKColors.Gold));
        ChartViewModels.Add(ledChart.Name, ledChart);
    }

    public void Fill(ExpChartsData data)
    {
        foreach (var result in data.Result)
        {
            string[] key = result.Key.Split("&");

            result.Value.ForEach(value => { AddPoint(key[0], key[1], value); });
        }
    }

    public void AddPoint(string chartName, string seriesName, double[] point)
    {
        ChartViewModels.TryGetValue(chartName, out ChartViewModel? chart);

        if (chart == null)
        {
            throw new Exception($"Chart \"{chartName}\" not found");
        }

        chart.AddPoint(seriesName, point);
    }

    public List<ChartTab> GetTabs()
    {
        return new List<ChartTab>
        {
            new()
            {
                Name = "Температура и люминесценция",
                Charts = new List<ChartViewModel> { ChartViewModels[Tal0], ChartViewModels[Tal1] }
            },
            new() { Name = "ТЛ кривая", Charts = new List<ChartViewModel> { ChartViewModels[Tl] } },
            new() { Name = "ОСЛ", Charts = new List<ChartViewModel> { ChartViewModels[Osl] } },
            new() { Name = "Светодиод", Charts = new List<ChartViewModel> { ChartViewModels[Led] } }
        };
    }

    public ExpChartsData GetData()
    {
        return ChartViewModels.Values.Aggregate(
            new ExpChartsData(),
            (expChartsData, chartViewModel) =>
            {
                chartViewModel.Series.ForEach(series =>
                {
                    foreach (ObservablePoint point in series.Values!)
                    {
                        expChartsData.AddPoint(
                            chartViewModel.Name,
                            series.Name!,
                            chartViewModel.XAxes[0].Name!,
                            chartViewModel.YAxes[0].Name!,
                            new[] { (double)point.X!, (double)point.Y! }
                        );
                    }
                });

                return expChartsData;
            }
        );
    }

    public void Clear()
    {
        foreach (var chartViewModel in ChartViewModels.Values)
        {
            chartViewModel.Clear();
        }
    }
}