using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Luminescence.Dialog;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class HeaderViewModel : BaseViewModel
{
    public ICommand ToggleActiveCommand { get; }
    public ICommand OpenCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand OpenSettingsDialogCommand { get; }
    public ICommand OpenTestDialogCommand { get; }

    public bool Connected
    {
        get => _connected;
        set => this.RaiseAndSetIfChanged(ref _connected, value);
    }

    public bool InProcess
    {
        get => _inProcess;
        set => this.RaiseAndSetIfChanged(ref _inProcess, value);
    }

    public bool PlayEnabled
    {
        get => _playEnabled;
        set => this.RaiseAndSetIfChanged(ref _playEnabled, value);
    }

    public bool StopEnabled
    {
        get => _stopEnabled;
        set => this.RaiseAndSetIfChanged(ref _stopEnabled, value);
    }

    public bool TestMode
    {
        get => _testMode;
        set => this.RaiseAndSetIfChanged(ref _testMode, value);
    }

    public BehaviorSubject<bool> OpenEnabled => _expChartService.OpenEnabled;
    public BehaviorSubject<bool> SaveEnabled => _expChartService.SaveEnabled;


    private bool _connected;
    private bool _inProcess;
    private bool _playEnabled;
    private bool _stopEnabled;
    private bool _testMode;

    private readonly ExpDevice _expDevice;
    private readonly ExpChartService _expChartService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;

    public HeaderViewModel(
        DialogService dialogService,
        ExpDevice expDevice,
        ExpChartService expChartService,
        MeasurementSettingsFormService measurementSettingsFormService
    )
    {
        _expDevice = expDevice;
        _expChartService = expChartService;
        _measurementSettingsFormService = measurementSettingsFormService;

        TestMode = _expDevice.TestMode;

        _expDevice.Connected
            .Subscribe(connected => { Connected = connected; });

        _expDevice.InProcess
            .Subscribe(inProcess => { InProcess = inProcess; });

        this.WhenAnyValue(x => x.Connected, x => x.InProcess)
            .Subscribe(result =>
            {
                var connected = result.Item1;
                var inProcess = result.Item2;

                PlayEnabled = connected && !inProcess;
                StopEnabled = connected && inProcess;
            });

        // Observable.Merge(new[] { _expDeviceService.Connected, _expDeviceService.InProcess })
        //     .Subscribe(result =>
        //     {
        //         var connected = result[0];
        //         var inProcess = result[1];
        //
        //         PlayEnabled = connected && !inProcess;
        //         StopEnabled = connected && inProcess;
        //     });

        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
        OpenCommand = ReactiveCommand.Create<Unit>(_ => _expChartService.Open());
        SaveCommand = ReactiveCommand.Create<Unit>(_ => _expChartService.Save());
        OpenSettingsDialogCommand = ReactiveCommand.Create<Unit>(_ =>
        {
            if (!_expDevice.InProcess.Value)
            {
                dialogService.Create<SettingsDialogViewModel>().Open();
            }
        });
        OpenTestDialogCommand = ReactiveCommand.Create<Unit>(_ =>
        {
            dialogService.Create<TestDialogViewModel>().Open();
        });
    }

    private void ToggleActive()
    {
        if (!InProcess)
        {
            _measurementSettingsFormService.GetModelFromStorage()
                .Where(model => model != null)
                .Subscribe(model => { _expDevice.RunProcess(model.ToDto()); });

            return;
        }

        _expDevice.StopProcess();
    }
}