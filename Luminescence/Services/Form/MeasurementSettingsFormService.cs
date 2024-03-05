using System;
using System.Linq;
using System.Reactive.Linq;
using Luminescence.Form;
using Luminescence.Models;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class MeasurementSettingsFormService
    : FormService<MeasurementSettingsFormViewModel, MeasurementSettingsFormModel>
{
    private ExpDeviceService _expDeviceService;

    public MeasurementSettingsFormService(ExpDeviceService expDeviceService)
    {
        _expDeviceService = expDeviceService;
    }

    public override void Initialize(MeasurementSettingsFormViewModel model)
    {
        base.Initialize(model);

        model.Controls
            .Select(control => control.Value.ValueChanges)
            .Merge()
            .Subscribe(_ =>
            {
                // ExpWriteDto dto = ToDto(model.ToModel());
                // model.Test = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(structure)).ToString();
                //
                // _expDeviceService.SendData(dto);
            });

        _expDeviceService.CurrentData
            .Subscribe((ExpReadDto data) =>
            {
                // model.Description = System.Text.Json.Nodes.JsonNode
                //     .Parse(JsonConvert.SerializeObject(data))
                //     .ToString();
            });
    }

    private ExpWriteDto ToDto(MeasurementSettingsFormModel model)
    {
        ExpWriteDto dto = new();
        dto.ID_Report = 1;
        dto.Command = 1;
        dto.Parameter0 = 0;
        dto.Parameter1 = 0;
        dto.HeaterMode = model.GetHeaterMode();
        dto.LEDMode = model.GetLEDMode();
        dto.PEMMode = model.GetPEMMode();
        dto.HeatingRate = Convert.ToByte(model.HeatingRate * 10); //  0.1 - 10 скорость нагрева
        dto.TemperatureError = 0;
        dto.LEDCurrentRate = Convert.ToByte(model.LEDCurrentRate * 10); // 0.1 - 500
        dto.StartTemperature = 0;
        dto.EndTemperature = Convert.ToByte(model.EndTemperature);
        dto.StartLEDCurrent = Convert.ToByte(model.StartLEDCurrent);
        dto.EndLEDCurrent = Convert.ToByte(model.EndLEDCurrent);
        dto.Upem = Convert.ToByte(model.Ufeu * 100); // default 0.5 (огр: 0.5 до 1.1 включ) (Ufeu*100)
        dto.KeyControl = 0;
        dto.PEMError = 0;
        dto.OffsetADCThermocouple = 0;
        dto.OffsetDACLED = 0;
        dto.CoefADCTemperature = 0;
        dto.CoefDACLED = 0;
        dto.Data = 0;
        dto.fError = 0;

        return dto;
    }
}