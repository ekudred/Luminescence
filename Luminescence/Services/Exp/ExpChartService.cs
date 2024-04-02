using System;
using System.Collections.Generic;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class ExpChartService
{
    public Dictionary<string, ChartViewModel> ChartViewModels = new();

    private readonly ExpDeviceService _expDeviceService;
    private readonly ExpChartsData _expChartsData;

    public ExpChartService(
        ExpDeviceService expDeviceService,
        ExpChartsData expChartsData
    )
    {
        _expDeviceService = expDeviceService;
        _expChartsData = expChartsData;
    }

    public void Initialize()
    {
        SetCharts();
        OnChangeData();
    }

    private void SetCharts()
    {
        ChartViewModel chartTemperatureTime = new(new("Время, сек"), new("Температура, °C"));
        chartTemperatureTime.AddSeries(new("0"));
        ChartViewModels.Add(ExpChart.TemperatureTime, chartTemperatureTime);

        ChartViewModel chartIntensityTime = new(new("Время, сек"), new("Интенсивность"));
        chartIntensityTime.AddSeries(new("0"));
        ChartViewModels.Add(ExpChart.IntensityTime, chartIntensityTime);

        ChartViewModel chartIntensityTemperature = new(new("Температура, °C"), new("Интенсивность"));
        chartIntensityTemperature.AddSeries(new("0"));
        ChartViewModels.Add(ExpChart.IntensityTemperature, chartIntensityTemperature);

        ChartViewModel chartIntensityCurrent = new(new("Ток светодиода, мА"), new("Интенсивность"));
        chartIntensityCurrent.AddSeries(new("0"));
        ChartViewModels.Add(ExpChart.IntensityCurrent, chartIntensityCurrent);
    }

    private void OnChangeData()
    {
        _expDeviceService.CurrentData
            .Subscribe(data =>
            {
                AddPoint(ExpChart.TemperatureTime, "0", new ObservablePoint(data.Counter, data.OpTemperature));
                AddPoint(ExpChart.IntensityTime, "0", new ObservablePoint(data.Counter, data.Intensity));
                AddPoint(ExpChart.IntensityTemperature, "0", new ObservablePoint(data.Intensity, data.Temperature));
                AddPoint(ExpChart.IntensityCurrent, "0", new ObservablePoint(data.Intensity, data.LEDCurrent));
            });

        // Random random = new Random();
        // double counter = 0.5;
        //
        // Observable
        //     .Interval(TimeSpan.FromMilliseconds(500))
        //     .Subscribe(_ =>
        //     {
        //         var value = random.NextDouble() * 100;
        //
        //         AddPoint(ExpChart.TemperatureTime, "0", counter, value);
        //
        //         counter += 0.5;
        //     });
    }

    private void AddPoint(string chartName, string seriesName, IChartEntity chartEntity)
    {
        ChartViewModels.TryGetValue(chartName, out ChartViewModel chart);

        if (chart == null)
        {
            throw new Exception($"Chart \"{chartName}\" not found");
        }

        double[] point = { chartEntity.Coordinate.PrimaryValue, chartEntity.Coordinate.SecondaryValue };

        chart.AddPoint(seriesName, point[0], point[1]);

        string key = $"{chartName}/{seriesName}";

        _expChartsData.Data.TryGetValue(key, out List<double[]>? value);

        if (value == null)
        {
            _expChartsData.Data.Add(key, new() { point });
        }
        else
        {
            value.Add(point);
        }
    }
}