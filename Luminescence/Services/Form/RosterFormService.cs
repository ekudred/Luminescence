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
                _expUsbDeviceService.PushData(ModelToStructure(model.ToModel()));
            });

        this.WhenAnyValue(x => x._expUsbDeviceService.Data)
            .Subscribe((ReadableDataStructure data) =>
            {
                var opt = new JsonSerializerOptions(){ WriteIndented=true };
                model.Description = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data)).ToString();;
                // model.Description = data.Temperature + " " + data.Intensity + " " + data.Upem;
                // model.Temperature = data.Temperature;
                // model.Code = data.Intensity;
                // model.VoltagePmt = data.Upem;
                // model.Current = data.LEDCurrent;

                // model.Description = data JSON;
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
        //
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