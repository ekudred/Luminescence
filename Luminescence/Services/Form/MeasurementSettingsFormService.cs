using System;
using System.Reactive.Linq;
using Luminescence.Form;
using Luminescence.Models;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class MeasurementSettingsFormService
    : FormService<MeasurementSettingsFormViewModel, MeasurementSettingsFormModel>
{
    private readonly string _storageName = typeof(MeasurementSettingsFormModel).Name;

    private readonly ExpDevice _expDevice;
    private readonly StorageService _storageService;

    public MeasurementSettingsFormService(
        ExpDevice expDevice,
        StorageService storageService
    )
    {
        _expDevice = expDevice;
        _storageService = storageService;
    }

    // public override void Initialize(MeasurementSettingsFormViewModel model)
    // {
    //     base.Initialize(model);
    //
    //     // _expDeviceService.CurrentData
    //     //     .Subscribe((ExpReadDto data) =>
    //     //     {
    //     //         // model.Description = System.Text.Json.Nodes.JsonNode
    //     //         //     .Parse(JsonConvert.SerializeObject(data))
    //     //         //     .ToString();
    //     //     });
    // }

    public IObservable<MeasurementSettingsFormModel> SetModelToStorage(MeasurementSettingsFormModel formModel)
    {
        return _storageService.Set(_storageName, formModel);
    }

    public IObservable<MeasurementSettingsFormModel?> GetModelFromStorage()
    {
        return _storageService.Get<MeasurementSettingsFormModel>(_storageName);
    }

    protected override IObservable<MeasurementSettingsFormModel?> Fill(MeasurementSettingsFormViewModel formViewModel)
    {
        return GetModelFromStorage()
            .Select(formModel =>
            {
                if (formModel == null)
                {
                    return SetModelToStorage(formViewModel.ToModel());
                }

                return Observable.Return(formModel);
            })
            .Switch();
    }
}