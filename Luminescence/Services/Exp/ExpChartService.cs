using System.Collections.Generic;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Services;

public class ExpChartService : ReactiveObject
{
    public Dictionary<string, ChartViewModel> ChartViewModels = new();

    private readonly ExpDeviceService _expDeviceService;

    public ExpChartService(ExpDeviceService expDeviceService)
    {
        _expDeviceService = expDeviceService;
    }

    public void Initialize()
    {
        ChartViewModels.Add(ExpChart.TemperatureTime, new("Время, сек", "Температура, °C"));
        ChartViewModels.Add(ExpChart.IntensityTime, new("Время, сек", "Интенсивность"));
        ChartViewModels.Add(ExpChart.IntensityTemperature, new("Температура, °C", "Интенсивность"));
        ChartViewModels.Add(ExpChart.IntensityCurrent, new("Ток светодиода, мА", "Интенсивность"));

        // _expDeviceService.CurrentData
        //     .Subscribe(data =>
        //     {
        //         ChartTemperatureTimeViewModel.AddPoint("Name", data.Counter, data.OpTemperature);
        //         // ChartTemperatureTimeViewModel.AddValue("Name", data.Counter, data.Intensity);
        //         // ChartIntensityTemperatureViewModel.AddValue("Name", data.Temperature, data.Counter);
        //         // ChartIntensityCurrentViewModel.AddValue("Name", data.LEDCurrent, data.Intensity);
        //     });
    }
}