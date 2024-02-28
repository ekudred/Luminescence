using System;
using System.Linq;
using System.Reactive.Linq;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class OptionsDialogFormService : FormService<OptionsDialogFormViewModel, OptionsDialogFormModel>
{
    private ExpDeviceService _expDeviceService;
    
    public OptionsDialogFormService(ExpDeviceService expDeviceService)
    {
        _expDeviceService = expDeviceService;
    }
    
    public override void Initialize(OptionsDialogFormViewModel model)
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
    }

    private ExpWriteData ModelToStructure(OptionsDialogFormModel model)
    {
        ExpWriteData @struct = new ExpWriteData();
        // structure.ID_Report =
        // structure.Command =
        // structure.Parameter0 = 
        // structure.Parameter1 = 
        // structure.HeaterMode =
        // structure.LEDMode =
        // structure.PEMMode =
        // structure.HeatRate =
        // structure.StartTemperature =
        // structure.EndTemperature =
        // structure.StartLEDCurrent =
        // structure.EndLEDCurrent =
        // structure.LEDCurrentRate =
        // structure.Upem =
        // structure.Data =
        // structure.fError = 

        return @struct;
    }
}