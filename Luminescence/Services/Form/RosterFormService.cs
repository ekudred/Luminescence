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
        ExpWriteDto dto = new ExpWriteDto();
        dto.ID_Report = 1;
        dto.Command = 1;
        dto.Parameter0 = 0;
        dto.Parameter1 = 0;

        if (model.HeaterOff)
        {
            dto.HeaterMode = 0;
        }

        if (model.LinearHeating)
        {
            dto.HeaterMode = 1;
        }

        if (model.TemperatureMaintenance)
        {
            dto.HeaterMode = 2;
        }

        // dto.LEDMode; // как HeaterMode ОСЛ вкладка
        // // Label -Напряжение на ФЭУ
        // dto.PEMMode = 1; // Автомат = 1 | Упр напря = 2
        // dto.HeatingRate = model.HeatRate*10; //  0.1 - 10 скорость нагрева
        // dto.TemperatureError; // -
        // dto.LEDCurrentRate = model.LEDCurrentRate * 10; // 0.1 - 500
        // dto.StartTemperature; // -
        // dto.EndTemperature = model.EndTemperature; // +
        // dto.StartLEDCurrent = model.StartLEDCurrent; // +
        // dto.EndLEDCurrent = model.EndLEDCurrent; // +
        // dto.Upem; // default 0.5 (огр: 0.5 до 1.1 включ) (Label*100)
        // dto.KeyControl; // -
        // dto.PEMError; // -
        // dto.OffsetADCThermocouple; // -
        // dto.OffsetDACLED; // -
        // dto.CoefADCTemperature; // -
        // dto.CoefDACLED; // -
        // // dto.Data;
        // // dto.fError;

        return dto;
    }
}