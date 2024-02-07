using System;
using Luminescence.Models;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartPanelViewModel : BaseViewModel
{
    public ChartModel ChartTemperatureTimeModel => _expChartService.ChartTemperatureTimeModel;
    public ChartModel ChartIntensityTimeModel => _expChartService.ChartIntensityTimeModel;
    public ChartModel ChartIntensityTemperatureModel => _expChartService.ChartIntensityTemperatureModel;
    public ChartModel ChartIntensityCurrentModel => _expChartService.ChartIntensityCurrentModel;

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

    private double _fullWidth = 0;
    private double _halfWidth = 0;
    private double _height = 0;

    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly ExpChartService _expChartService;

    public ChartPanelViewModel(
        MainWindowViewModel mainWindowViewModel,
        ExpChartService expChartService
    )
    {
        _mainWindowViewModel = mainWindowViewModel;
        _expChartService = expChartService;
        
        InitializeChartSizes();
    }

    private void InitializeChartSizes()
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