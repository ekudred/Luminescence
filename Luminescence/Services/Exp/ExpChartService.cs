using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Platform.Storage;
using Luminescence.Dialog;
using Luminescence.Utils;
using Luminescence.ViewModels;
using SkiaSharp;

namespace Luminescence.Services;

public class ExpChartService
{
    public readonly Dictionary<string, ChartViewModel> ChartViewModels = new();

    public readonly BehaviorSubject<bool> OpenEnabled = new(true);
    public readonly BehaviorSubject<bool> SaveEnabled = new(false);

    private readonly Dictionary<string, List<double[]>> Data = new();

    private readonly ExpDeviceService _expDeviceService;
    private readonly MeasurementSettingsFormViewModel _measurementSettingsFormViewModel;
    private readonly AppFilePickerService _appFilePickerService;
    private readonly SystemDialogService _systemDialogService;
    private readonly DialogService _dialogService;

    public ExpChartService(
        ExpDeviceService expDeviceService,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel,
        AppFilePickerService appFilePickerService,
        SystemDialogService systemDialogService,
        DialogService dialogService
    )
    {
        _expDeviceService = expDeviceService;
        _measurementSettingsFormViewModel = measurementSettingsFormViewModel;
        _appFilePickerService = appFilePickerService;
        _systemDialogService = systemDialogService;
        _dialogService = dialogService;

        _expDeviceService.InProcess
            .Subscribe(inProcess =>
            {
                OpenEnabled.OnNext(!inProcess);
                SaveEnabled.OnNext(!inProcess && Data.Count > 0);
            });
    }

    public void Initialize()
    {
        SetCharts(ChartViewModels);
        OnChangeData();
    }

    public void Open()
    {
        if (!OpenEnabled.Value)
        {
            return;
        }

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
            .Select(data =>
            {
                return _systemDialogService.Confirm(new ConfirmationDialogData("Открыть в новом окне?"))
                    .Select(confirm =>
                    {
                        if (confirm)
                        {
                            IDialogWindow<ChartPanelDialogViewModel> dialog =
                                _dialogService.Create<ChartPanelDialogViewModel>();

                            var chartTabsViewModel = new ChartTabsViewModel
                            {
                                Charts = new Dictionary<string, ChartViewModel>()
                            };
                            SetCharts(chartTabsViewModel.Charts);

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
                                    AddPoint(chartName, seriesName, xValues[i], yValues[i], chartTabsViewModel.Charts);
                                }
                            }

                            var o = new ChartPanelDialogData(chartTabsViewModel);
                            dialog.ViewModel.Initialize(o);

                            dialog.Open();

                            return null;
                        }

                        return data;
                    });
            }).Switch()
            .Where(data => data != null)
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
                        AddPoint(chartName, seriesName, xValues[i], yValues[i], ChartViewModels, Data);
                    }
                }
            });
    }

    public void Save()
    {
        if (!SaveEnabled.Value)
        {
            return;
        }

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

                    return acc +
                           $"{splitKey[0]}/{splitKey[1]}\n{axisNames[0]}: {xValues}\n{axisNames[1]}: {yValues}{end}";
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

    private void SetCharts(Dictionary<string, ChartViewModel> chartViewModels)
    {
        ChartViewModel chartTemperatureTime = new(new("Время", "сек"), new("Температура", "°C"));
        chartTemperatureTime.AddSeries(new("0"));
        chartTemperatureTime.AddSeries(new("1", SKColors.Gold));
        chartViewModels.Add(ExpChart.TemperatureTime, chartTemperatureTime);

        ChartViewModel chartIntensityTime = new(new("Время", "сек"), new("Интенсивность", "УЕ"));
        chartIntensityTime.AddSeries(new("0"));
        chartViewModels.Add(ExpChart.IntensityTime, chartIntensityTime);

        ChartViewModel chartIntensityTemperature = new(new("Температура", "°C"), new("Интенсивность", "УЕ"));
        chartIntensityTemperature.AddSeries(new("0"));
        chartViewModels.Add(ExpChart.IntensityTemperature, chartIntensityTemperature);

        ChartViewModel chartIntensityCurrent = new(new("Ток светодиода", "мА"), new("Время", "сек"));
        chartIntensityCurrent.AddSeries(new("0"));
        chartViewModels.Add(ExpChart.IntensityCurrent, chartIntensityCurrent);
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
                AddPoint(ExpChart.TemperatureTime, "0", data.Counter, data.Temperature, ChartViewModels, Data);
                AddPoint(ExpChart.TemperatureTime, "1", data.Counter, data.OpTemperature, ChartViewModels, Data);

                AddPoint(ExpChart.IntensityTime, "0", data.Counter, data.Intensity, ChartViewModels, Data);

                AddPoint(ExpChart.IntensityTemperature, "0", data.OpTemperature, data.Intensity, ChartViewModels, Data);

                AddPoint(ExpChart.IntensityCurrent, "0", data.OpLEDCurrent, data.Intensity, ChartViewModels, Data);
                // + вкладка оплед и лед
                // AddPoint(ExpChart.IntensityCurrent, "1", data.OpLEDCurrent, data.Counter, ChartViewModels, Data);
            });
    }

    private void AddPoint(string chartName, string seriesName, double xValue, double yValue,
        Dictionary<string, ChartViewModel> chartViewModels, Dictionary<string, List<double[]>>? data = null)
    {
        chartViewModels.TryGetValue(chartName, out ChartViewModel chart);

        if (chart == null)
        {
            throw new Exception($"Chart \"{chartName}\" not found");
        }

        double[] point = { xValue, yValue };

        chart.AddPoint(seriesName, point[0], point[1]);

        if (data == null)
        {
            return;
        }

        string key = $"{chartName}/{seriesName}/{chart.XAxes[0].Name}&{chart.YAxes[0].Name}";

        data.TryGetValue(key, out List<double[]>? value);

        if (value == null)
        {
            data.Add(key, new() { point });
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