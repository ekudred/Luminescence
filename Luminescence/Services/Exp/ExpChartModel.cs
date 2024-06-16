using System;
using Luminescence.Shared.Utils;

namespace Luminescence.Services;

public class ExpChartModel
{
    public static ExpChartModel PrepareDto(ExpReadDto expReadDto)
    {
        return new ExpChartModel(expReadDto);
    }

    public double Counter { get; set; }
    public double Intensity { get; set; }
    public double HeaterMode { get; set; }
    public double HeatingRate { get; set; }
    public double LEDCurrentRate { get; set; }
    public double OpTemperature { get; set; }
    public double Temperature { get; set; }
    public double OpLEDCurrent { get; set; }
    public double LEDCurrent { get; set; }
    public double LEDMode { get; set; }
    public double Upem { get; set; }
    public double AutoUpem { get; set; }

    public ExpChartModel(ExpReadDto expReadDto)
    {
        Counter = Math.Round((double)expReadDto.Counter.ToDouble()!, 1);
        Intensity = Math.Round((double)expReadDto.Intensity.ToDouble()!, 1);
        Temperature = Math.Round((double)expReadDto.Temperature.ToDouble()!, 1);
        OpTemperature = Math.Round((double)expReadDto.OpTemperature.ToDouble()!, 1);
        OpLEDCurrent = Math.Round((double)expReadDto.OpLEDCurrent.ToDouble()!, 1);
        LEDCurrent = Math.Round((double)expReadDto.LEDCurrent.ToDouble()!, 1);
    }
}