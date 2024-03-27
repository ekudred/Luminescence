using System;
using System.Collections.Generic;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class ExpChartService
{
    public Dictionary<string, ChartViewModel> ChartViewModels = new();

    private readonly string _storageName = "Charts";

    private readonly ExpDeviceService _expDeviceService;
    private readonly StorageService _storageService;
    private readonly ExpChartsData _expChartsData;

    public ExpChartService(
        ExpDeviceService expDeviceService,
        StorageService storageService,
        ExpChartsData expChartsData
    )
    {
        _expDeviceService = expDeviceService;
        _storageService = storageService;
        _expChartsData = expChartsData;
    }

    public void Initialize()
    {
        SetCharts();
        OnChangeData();
    }

    private void SetCharts()
    {
        ChartViewModel chartTemperatureTime = new("Время, сек", "Температура, °C");
        chartTemperatureTime.AddSeries("0");
        ChartViewModels.Add(ExpChart.TemperatureTime, chartTemperatureTime);

        ChartViewModel chartIntensityTime = new("Время, сек", "Интенсивность");
        chartIntensityTime.AddSeries("0");
        ChartViewModels.Add(ExpChart.IntensityTime, chartIntensityTime);

        ChartViewModel chartIntensityTemperature = new("Температура, °C", "Интенсивность");
        chartIntensityTemperature.AddSeries("0");
        ChartViewModels.Add(ExpChart.IntensityTemperature, chartIntensityTemperature);

        ChartViewModel chartIntensityCurrent = new("Ток светодиода, мА", "Интенсивность");
        chartIntensityCurrent.AddSeries("0");
        ChartViewModels.Add(ExpChart.IntensityCurrent, chartIntensityCurrent);
    }

    private void OnChangeData()
    {
        _expDeviceService.CurrentData
            .Subscribe(data =>
            {
                AddPoint(ExpChart.TemperatureTime, "0", data.Counter, data.OpTemperature);
                AddPoint(ExpChart.IntensityTime, "0", data.Counter, data.Intensity);
                AddPoint(ExpChart.IntensityTemperature, "0", data.Intensity, data.Temperature);
                AddPoint(ExpChart.IntensityCurrent, "0", data.Intensity, data.LEDCurrent);
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

    private void AddPoint(string chartName, string seriesName, double xValue, double yValue)
    {
        ChartViewModels.TryGetValue(chartName, out ChartViewModel chart);

        if (chart == null)
        {
            throw new Exception($"Chart \"{chartName}\" not found");
        }

        chart.AddPoint(seriesName, xValue, yValue);

        string key = $"{chartName}/{seriesName}";
        string[] point = { xValue.ToString(), yValue.ToString() };

        _expChartsData.Data.TryGetValue(key, out List<string[]>? value);

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