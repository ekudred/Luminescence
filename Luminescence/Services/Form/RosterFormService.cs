using System;
using System.Linq;
using System.Reactive.Linq;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.ViewModels;
using ReactiveUI;

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
            .Distinct()
            .Throttle(new TimeSpan(400))
            .Subscribe(_ =>
            {
                // _expUsbDeviceService.PushData(ModelToStructure(model.ToModel()));
            });

        this.WhenAnyValue(x => x._expUsbDeviceService.Data)
            .Subscribe((ReadableDataStructure data) =>
            {
                model.Description = data.Temperature + " " + data.Intensity + " " + data.Upem;
                model.Temperature = data.Temperature;
                model.Code = data.Intensity;
                model.VoltagePmt = data.Upem;
                model.Current = data.LEDCurrent;
            });
    }

    private WritableDataStructure ModelToStructure(RosterFormModel model)
    {
        // TODO model - RosterFormModel будет изменена, тк RadioButtonViewModel неправильно написана, но часть полей будет заполняться
        WritableDataStructure structure = new WritableDataStructure();
        // structure.ID_Report =
        // structure.Command =
        // structure.Parameter0 =
        // structure.Parameter1 =

        // TODO HeaterOff LedOff TemperatureMaintenance
        // structure.HeaterMode = Convert.ToUInt32(model.HeaterOff);
        // structure.LEDMode = Convert.ToUInt32(model.LedOff);

        // TODO Automatic и что то еще
        // structure.PEMMode =

        structure.HeatRate = Convert.ToUInt32(model.HeatRate);
        // structure.StartTemperature =
        structure.EndTemperature = Convert.ToUInt32(model.EndTemperature);
        structure.StartLEDCurrent = Convert.ToUInt32(model.StartLEDCurrent);
        structure.EndLEDCurrent = Convert.ToUInt32(model.EndLEDCurrent);
        structure.LEDCurrentRate = Convert.ToUInt32(model.LEDCurrentRate);
        structure.Upem = Convert.ToUInt32(model.Upem);
        // structure.Data =
        // structure.fError = 

        return structure;
    }
}