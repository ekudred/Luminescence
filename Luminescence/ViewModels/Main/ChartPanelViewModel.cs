using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using Luminescence.Models;
using Luminescence.Services;
using Newtonsoft.Json;
using ReactiveUI;
using SkiaSharp;

namespace Luminescence.ViewModels;

public class ChartPanelViewModel : BaseViewModel
{
    public double FullWidth
    {
        get => _fullWidth;
        set => this.RaiseAndSetIfChanged(ref _fullWidth, value);
    }

    public double HalfWidth
    {
        get => _halfWidth;
        set => this.RaiseAndSetIfChanged(ref _halfWidth, value);
    }

    public double Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }
    public string Test
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }

    private double _fullWidth;
    private double _halfWidth;
    private double _height;
    private string _test;

    public Dictionary<string, ChartViewModel> Charts => _expChartService.ChartViewModels;

    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly ExpChartService _expChartService;

    public ChartPanelViewModel(
        MainWindowViewModel mainWindowViewModel,
        ExpChartService expChartService,
        ExpDeviceService expDeviceService
    )
    {
        _mainWindowViewModel = mainWindowViewModel;
        _expChartService = expChartService;

        _expChartService.Initialize();

        OnChangeChartSizes();

        expDeviceService.CurrentData
            .Subscribe(data =>
            {
                Test = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data)).ToString();
            });
    }

    private void OnChangeChartSizes()
    {
        this.WhenAnyValue(viewModel => viewModel._mainWindowViewModel.Width)
            .Subscribe(width =>
                {
                    FullWidth = width;
                    HalfWidth = FullWidth / 2;
                }
            );

        this.WhenAnyValue(viewModel => viewModel._mainWindowViewModel.Height)
            .Subscribe(height => { Height = height - 228; });
    }
}