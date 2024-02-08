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
            .Subscribe(_ => { model.ToModel(); });

        this.WhenAnyValue(x => x._expUsbDeviceService.Data)
            .Subscribe((ReadableDataStructure data) =>
            {
                model.Description = data.Temperature + " " + data.Intensity + " " + data.Upem;
                model.Temperature = data.Temperature;
                model.Code = data.Intensity;
                model.VoltagePmt = data.Upem;
                model.Current = data.LEDCurrent;
            });
        
        // _expUsbDeviceService.PushData();
    }
}