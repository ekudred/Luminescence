using System;
using System.Linq;
using System.Reactive.Linq;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.ViewModels;
using Newtonsoft.Json;

namespace Luminescence.Services;

public class RosterFormService : FormService<RosterFormViewModel, RosterFormModel>
{
    private ExpDeviceService _expDeviceService;

    public RosterFormService(ExpDeviceService expDeviceService)
    {
        _expDeviceService = expDeviceService;
    }

    public override void Initialize(RosterFormViewModel model)
    {
        base.Initialize(model);

        model.Controls
            .Select(control => control.Value.ValueChanges)
            .Merge()
            .Subscribe(_ =>
            {
                ExpWriteDto dto = ToDto(model.ToModel());
                // model.Test = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(structure)).ToString();
                //
                // _expDeviceService.SendData(dto);
            });

        _expDeviceService.CurrentData
            .Subscribe((ExpReadDto data) =>
            {
                model.Description = System.Text.Json.Nodes.JsonNode
                    .Parse(JsonConvert.SerializeObject(data))
                    .ToString();
            });
    }

    private ExpWriteDto ToDto(RosterFormModel model)
    {
        // TODO model - RosterFormModel будет изменена, тк RadioButtonViewModel неправильно написана, но часть полей будет заполняться
        ExpWriteDto dto = new ExpWriteDto();
        dto.ID_Report = 1;
        dto.Command = 1;
        dto.Parameter0 = 0;
        dto.Parameter1 = 0;
        //s
        // // TODO HeaterOff LedOff TemperatureMaintenance
        dto.HeaterMode = 1;
        // // structure.LEDMode = Convert.ToUInt32(model.LedOff);
        //
        // // TODO Automatic и что то еще
        // // structure.PEMMode =
        //
        // structure.HeatingRate = Convert.ToUInt32(model.HeatRate);
        dto.StartTemperature = 35;
        dto.EndTemperature = 150;
        // structure.StartLEDCurrent = Convert.ToUInt32(model.StartLEDCurrent);
        // structure.EndLEDCurrent = Convert.ToUInt32(model.EndLEDCurrent);
        // structure.LEDCurrentRate = Convert.ToUInt32(model.LEDCurrentRate);
        // structure.Upem = Convert.ToUInt32(model.Upem);
        // // structure.Data =
        // // structure.fError =

        return dto;
    }
}