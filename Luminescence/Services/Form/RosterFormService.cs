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
            // .Distinct()
            .Throttle(new TimeSpan(2000))
            .Subscribe(_ =>
            {
                var structure = ModelToStructure(model.ToModel());
                model.Test = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(structure)).ToString();

                _expDeviceService.SendData(structure);
            });

        _expDeviceService.CurrentData
            .Subscribe((ExpReadData data) =>
            {
                model.Description = System.Text.Json.Nodes.JsonNode
                    .Parse(JsonConvert.SerializeObject(data))
                    .ToString();
            });
    }

    private ExpWriteData ModelToStructure(RosterFormModel model)
    {
        // TODO model - RosterFormModel будет изменена, тк RadioButtonViewModel неправильно написана, но часть полей будет заполняться
        ExpWriteData @struct = new ExpWriteData();
        @struct.ID_Report = 1;
        @struct.Command = 1;
        @struct.Parameter0 = 0;
        @struct.Parameter1 = 0;
        //s
        // // TODO HeaterOff LedOff TemperatureMaintenance
        @struct.HeaterMode = 1;
        // // structure.LEDMode = Convert.ToUInt32(model.LedOff);
        //
        // // TODO Automatic и что то еще
        // // structure.PEMMode =
        //
        // structure.HeatingRate = Convert.ToUInt32(model.HeatRate);
        @struct.StartTemperature = 35;
        @struct.EndTemperature = 150;
        // structure.StartLEDCurrent = Convert.ToUInt32(model.StartLEDCurrent);
        // structure.EndLEDCurrent = Convert.ToUInt32(model.EndLEDCurrent);
        // structure.LEDCurrentRate = Convert.ToUInt32(model.LEDCurrentRate);
        // structure.Upem = Convert.ToUInt32(model.Upem);
        // // structure.Data =
        // // structure.fError =

        return @struct;
    }
}