using System;
using System.Linq;
using System.Reactive.Linq;
using Luminescence.Models;
using Luminescence.Form;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class OptionsDialogFormService : FormService<OptionsDialogFormViewModel, OptionsDialogFormModel>
{
    private ExpUsbDeviceService _expUsbDeviceService;
    
    public OptionsDialogFormService(ExpUsbDeviceService expUsbDeviceService)
    {
        _expUsbDeviceService = expUsbDeviceService;
    }
    
    public override void Initialize(OptionsDialogFormViewModel model)
    {
        base.Initialize(model);

        model.Controls
            .Select(control => control.Value.ValueChanges)
            .Merge()
            .Distinct()
            .Throttle(new TimeSpan(400))
            .Subscribe(_ => { model.ToModel(); });
        
        // _expUsbDeviceService.PushData();
    }

    public void ApplyChanges()
    {
    }

    public void CancelChanges()
    {
    }
}