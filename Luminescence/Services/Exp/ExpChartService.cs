using System;
using Luminescence.Models;
using ReactiveUI;

namespace Luminescence.Services;

public class ExpChartService : ReactiveObject
{
    public ChartModel ChartTemperatureTimeModel
    {
        get => _chartTemperatureTimeModel;
        private set => this.RaiseAndSetIfChanged(ref _chartTemperatureTimeModel, value);
    }

    public ChartModel ChartIntensityTimeModel
    {
        get => _chartIntensityTimeModel;
        private set => this.RaiseAndSetIfChanged(ref _chartIntensityTimeModel, value);
    }

    public ChartModel ChartIntensityTemperatureModel
    {
        get => _chartIntensityTemperatureModel;
        private set => this.RaiseAndSetIfChanged(ref _chartIntensityTemperatureModel, value);
    }

    public ChartModel ChartIntensityCurrentModel
    {
        get => _chartIntensityCurrentModel;
        private set => this.RaiseAndSetIfChanged(ref _chartIntensityCurrentModel, value);
    }

    public ChartModel _chartTemperatureTimeModel;
    public ChartModel _chartIntensityTimeModel;
    public ChartModel _chartIntensityTemperatureModel;
    public ChartModel _chartIntensityCurrentModel;

    private readonly ExpUsbDeviceService _expUsbDeviceService;

    public ExpChartService(ExpUsbDeviceService expUsbDeviceService)
    {
        _expUsbDeviceService = expUsbDeviceService;
    }

    public void Initialize()
    {
        ChartTemperatureTimeModel = new ChartModel("Температура, °C", "Время, сек");
        ChartIntensityTimeModel = new ChartModel("Интенсивность", "Время, сек");
        ChartIntensityTemperatureModel = new ChartModel("Интенсивность", "Температура, °C");
        ChartIntensityCurrentModel = new ChartModel("Интенсивность", "Ток светодиода, мА");

        this.WhenAnyValue(x => x._expUsbDeviceService.Data)
            .Subscribe((ReadableDataStructure data) => { });
    }
}