using System;
using Luminescence.Chart;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartPanelViewModel : BaseViewModel
{
    public ChartModel ChartTemperatureTimeModel { get; set; }
    public ChartModel ChartIntensityTimeModel { get; set; }
    public ChartModel ChartIntensityTemperatureModel { get; set; }
    public ChartModel ChartIntensityCurrentModel { get; set; }

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

    private MainWindowViewModel _mainWindowViewModel { get; }

    public ChartPanelViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        InitializeChartModels();
        InitializeChartSizes();
    }

    private void InitializeChartModels()
    {
        ChartTemperatureTimeModel = new ChartModel("Температура, °C", "Время, сек");
        ChartIntensityTimeModel = new ChartModel("Интенсивность", "Время, сек");
        ChartIntensityTemperatureModel = new ChartModel("Интенсивность", "Температура, °C");
        ChartIntensityCurrentModel = new ChartModel("Интенсивность", "Ток светодиода, мА");
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