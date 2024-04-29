using System;
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

    private string _test;

    public TestDialogViewModel(ExpDevice expDevice)
    {
        expDevice.CurrentData
            .Subscribe(data =>
            {
                Text = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data))!.ToString();
            });
    }
}