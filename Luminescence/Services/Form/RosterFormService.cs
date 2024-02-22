using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text.Json;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Luminescence.Services;

public class RosterFormService : FormService<RosterFormViewModel, RosterFormModel>
{
    private ExpUsbDeviceService _expUsbDeviceService;

    public RosterFormService(ExpUsbDeviceService expUsbDeviceService)
    {
        _expUsbDeviceService = expUsbDeviceService;
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

                _expUsbDeviceService.PushData(structure);
            });

        this.WhenAnyValue(x => x._expUsbDeviceService.Data)
            .Subscribe((ReadableDataStructure data) =>
            {
                model.Description = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data)).ToString();
            });
    }

    private WritableDataStructure ModelToStructure(RosterFormModel model)
    {
        // TODO model - RosterFormModel будет изменена, тк RadioButtonViewModel неправильно написана, но часть полей будет заполняться
        WritableDataStructure structure = new WritableDataStructure();
        structure.ID_Report = 1;
        structure.Command = 1;
        structure.Parameter0 = 0;
        structure.Parameter1 = 0;
        //s
        // // TODO HeaterOff LedOff TemperatureMaintenance
        structure.HeaterMode = 1;
        // // structure.LEDMode = Convert.ToUInt32(model.LedOff);
        //
        // // TODO Automatic и что то еще
        // // structure.PEMMode =
        //
        // structure.HeatingRate = Convert.ToUInt32(model.HeatRate);
        structure.StartTemperature = 35;
        structure.EndTemperature = 150;
        // structure.StartLEDCurrent = Convert.ToUInt32(model.StartLEDCurrent);
        // structure.EndLEDCurrent = Convert.ToUInt32(model.EndLEDCurrent);
        // structure.LEDCurrentRate = Convert.ToUInt32(model.LEDCurrentRate);
        // structure.Upem = Convert.ToUInt32(model.Upem);
        // // structure.Data =
        // // structure.fError =

        return structure;
    }
}