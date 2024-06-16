using System;
using System.Reactive.Linq;
using Luminescence.Dialog;
using Luminescence.Services;
using Newtonsoft.Json;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class TestDialogViewModel : DialogBaseViewModel
{
    public string Text
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }

    public decimal IntensityMultiplier
    {
        get => _intensityMultiplier;
        set => this.RaiseAndSetIfChanged(ref _intensityMultiplier, value);
    }

    private decimal _intensityMultiplier;
    private string _test;

    public TestDialogViewModel(
        ExpDevice expDevice,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel
    )
    {
        expDevice.CurrentData
            .Subscribe(data =>
            {
                Text = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data))!.ToString();
            });

        IntensityMultiplier = measurementSettingsFormViewModel.IntensityMultiplier;

        measurementSettingsFormViewModel.FormChanged
            .Subscribe(_ => { IntensityMultiplier = measurementSettingsFormViewModel.IntensityMultiplier; });
    }
}