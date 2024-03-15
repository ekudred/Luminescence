using System;
using Luminescence.Services;
using Newtonsoft.Json;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class RosterViewModel : BaseViewModel
{
    public bool InProcess
    {
        get => _inProcess;
        private set => this.RaiseAndSetIfChanged(ref _inProcess, value);
    }

    public double Temperature
    {
        get => _temperature;
        set => this.RaiseAndSetIfChanged(ref _temperature, value);
    }

    public double VoltagePmt
    {
        get => _voltagePmt;
        set => this.RaiseAndSetIfChanged(ref _voltagePmt, value);
    }

    public double Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }

    public double Code
    {
        get => _code;
        set => this.RaiseAndSetIfChanged(ref _code, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public string Test
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }

    private double _temperature = 0;
    private double _voltagePmt = 0;
    private double _current = 0;
    private double _code = 404;
    private string _description = "";
    private string _test = "";
    private bool _inProcess;

    private readonly ExpDeviceService _expDeviceService;

    public RosterViewModel(ExpDeviceService expDeviceService)
    {
        _expDeviceService = expDeviceService;

        _expDeviceService.CurrentData
            .Subscribe(data =>
            {
                Test = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data)).ToString();
            });
    }
}