using System;
using Luminescence.Models;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Services;

public class ExpChartService : ReactiveObject
{
    public ChartViewModel ChartTemperatureTimeViewModel
    {
        get => _chartTemperatureTimeModel;
        private set => this.RaiseAndSetIfChanged(ref _chartTemperatureTimeModel, value);
    }

    public ChartViewModel ChartIntensityTimeViewModel
    {
        get => _chartIntensityTimeModel;
        private set => this.RaiseAndSetIfChanged(ref _chartIntensityTimeModel, value);
    }

    public ChartViewModel ChartIntensityTemperatureViewModel
    {
        get => _chartIntensityTemperatureModel;
        private set => this.RaiseAndSetIfChanged(ref _chartIntensityTemperatureModel, value);
    }

    public ChartViewModel ChartIntensityCurrentViewModel
    {
        get => _chartIntensityCurrentModel;
        private set => this.RaiseAndSetIfChanged(ref _chartIntensityCurrentModel, value);
    }

    public ChartViewModel _chartTemperatureTimeModel;
    public ChartViewModel _chartIntensityTimeModel;
    public ChartViewModel _chartIntensityTemperatureModel;
    public ChartViewModel _chartIntensityCurrentModel;

    private readonly ExpDeviceService _expDeviceService;

    public ExpChartService(ExpDeviceService expDeviceService)
    {
        _expDeviceService = expDeviceService;
    }

    public void Initialize()
    {
        ChartTemperatureTimeViewModel = new("Время, сек", "Температура, °C");
        ChartIntensityTimeViewModel = new("Время, сек", "Интенсивность");
        ChartIntensityTemperatureViewModel = new("Температура, °C", "Интенсивность");
        ChartIntensityCurrentViewModel = new("Ток светодиода, мА", "Интенсивность");

        _expDeviceService.CurrentData
            .Subscribe(data =>
            {
                ChartTemperatureTimeViewModel.AddValue("Name", data.Counter, data.Temperature);
                ChartTemperatureTimeViewModel.AddValue("Name", data.Counter, data.Intensity);
                ChartIntensityTemperatureViewModel.AddValue("Name", data.Temperature, data.Counter);
                ChartIntensityCurrentViewModel.AddValue("Name", data.LEDCurrent, data.Intensity);
            });
    }
}