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
            .Subscribe(_ =>
            {
                // _expUsbDeviceService.PushData(ToDto(model.ToModel()));
            });
    }

    private ExpWriteDto ToDto(OptionsDialogFormModel model)
    {
        ExpWriteDto dto = new ExpWriteDto();
        // dto.ID_Report =
        // dto.Command =
        // dto.Parameter0 =
        // dto.Parameter1 =
        // dto.HeaterMode =
        // dto.LEDMode =
        // dto.PEMMode =
        // dto.HeatRate =
        // dto.StartTemperature =
        // dto.EndTemperature =
        // dto.StartLEDCurrent =
        // dto.EndLEDCurrent =
        // dto.LEDCurrentRate =
        // dto.Upem =
        // dto.Data =
        // dto.fError =

        return dto;
    }
}