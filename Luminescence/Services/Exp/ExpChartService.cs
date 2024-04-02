using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Platform.Storage;
using DynamicData;
using Luminescence.Utils;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class ExpChartService
{
    public Dictionary<string, ChartViewModel> ChartViewModels = new();

    public readonly Dictionary<string, List<double[]>> Data = new();

    private readonly ExpDeviceService _expDeviceService;
    private readonly MeasurementSettingsFormViewModel _measurementSettingsFormViewModel;
    private readonly AppFilePickerService _appFilePickerService;

    public ExpChartService(
        ExpDeviceService expDeviceService,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel,
        AppFilePickerService appFilePickerService
    )
    {
        _expDeviceService = expDeviceService;
        _measurementSettingsFormViewModel = measurementSettingsFormViewModel;
        _appFilePickerService = appFilePickerService;
    }

    public void Initialize()
    {
        SetCharts();
        OnChangeData();
    }

    public void Open()
    {
        AppFilePickerOpenOptions options = new()
        {
            Title = "Получение значений всех графиков",
            FileTypeFilter = new[]
            {
                new FilePickerFileType("HTL")
                {
                    Patterns = new[] { "*.htl" },
                    AppleUniformTypeIdentifiers = new[] { "public.htl" },
                    MimeTypes = new[] { "text/htl" }
                }
            },
            StartLocationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        };
        _appFilePickerService.Open(options)
            .Subscribe(data =>
            {
                foreach (var a in ((string)data).Split("\n\n"))
                {
                    var c = a.Split("\n");
                    var splitC = c[0].Split("/");
                    var chartName = splitC[0];
                    var seriesName = splitC[1];

                    string[] values1 = c[1].Split(":")[1].Trim().Split(";");
                    double[] xValues = values1.Select(i => (double)i.ToDouble()!).ToArray();

                    string[] values2 = c[2].Split(":")[1].Trim().Split(";");
                    double[] yValues = values2.Select(i => (double)i.ToDouble()!).ToArray();

                    for (int i = 0; i < values1.Length - 1; i++)
                    {
                        AddPoint(chartName, seriesName, xValues[i], yValues[i]);
                    }
                }
            });
    }

    public void Save()
    {
        var result = Data
            .Select((item, i) => (item, i))
            .Aggregate("",
            (acc, item) =>
            {
                string[] splitKey = item.item.Key.Split("/");
                string[] axisNames = splitKey[2].Split("&");

                string xValues = string.Concat(
                    item.item.Value
                        .Select((value, i) =>
                        {
                            string end = i != item.item.Value.Count - 1 ? ";" : string.Empty;
                            return $"{value[0].ToString()}{end}";
                        })
                        .ToArray()
                );
                string yValues = string.Concat(
                    item.item.Value
                        .Select((value, i) =>
                        {
                            string end = i != item.item.Value.Count - 1 ? ";" : string.Empty;
                            return $"{value[1].ToString()}{end}";
                        })
                        .ToArray()
                );

                string end = item.i == Data.Count - 1 ? "" : "\n\n";

                return acc + $"{splitKey[0]}/{splitKey[1]}\n{axisNames[0]}: {xValues}\n{axisNames[1]}: {yValues}{end}";
            }
        );

        AppFilePickerSaveOptions options = new()
        {
            Title = "Сохранения значений всех графиков",
            FileName = "data",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("HTL")
                {
                    Patterns = new[] { "*.htl" },
                    AppleUniformTypeIdentifiers = new[] { "public.htl" },
                    MimeTypes = new[] { "text/htl" }
                }
            },
            DefaultExtension = "htl",
            StartLocationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        };
        _appFilePickerService.Save(result, options)
            .Subscribe();
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
        _expDeviceService.InProcess
            .CombineWithPrevious()
            .Where(inProcess =>
                inProcess.Current && !inProcess.Previous && _measurementSettingsFormViewModel.ToModel().Clear)
            .Subscribe(_ => { Clear(); });

        _expDeviceService.CurrentData
            .Subscribe(data =>
            {
                AddPoint(ExpChart.TemperatureTime, "0", data.Counter, data.Temperature);
                AddPoint(ExpChart.IntensityTime, "0", data.Counter, data.Intensity);
                AddPoint(ExpChart.IntensityTemperature, "0", data.Intensity, data.Temperature);
                AddPoint(ExpChart.IntensityCurrent, "0", data.Intensity, data.LEDCurrent);
            });
    }

    private void AddPoint(string chartName, string seriesName, double xValue, double yValue)
    {
        ChartViewModels.TryGetValue(chartName, out ChartViewModel chart);

        if (chart == null)
        {
            throw new Exception($"Chart \"{chartName}\" not found");
        }

        double[] point = { xValue, yValue };

        chart.AddPoint(seriesName, point[0], point[1]);

        string key = $"{chartName}/{seriesName}/{chart.XAxes[0].Name}&{chart.YAxes[0].Name}";

        Data.TryGetValue(key, out List<double[]>? value);

        if (value == null)
        {
            Data.Add(key, new() { point });
        }
        else
        {
            value.Add(point);
        }
    }

    private void Clear()
    {
        foreach (var chartViewModel in ChartViewModels.Values)
        {
            chartViewModel.Clear();
        }

        foreach (var item in Data.Values)
        {
            item.Clear();
        }
    }
}