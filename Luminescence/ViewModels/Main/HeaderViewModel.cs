using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Luminescence.Dialog;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class HeaderViewModel : BaseViewModel
{
    public ICommand OpenSettingsDialogCommand { get; }
    public ICommand ToggleActiveCommand { get; }
    public ICommand SaveCommand { get; }

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

    public string ConnectionStatus
    {
        get => _connectionStatus;
        set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
    }

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

    private bool _playEnabled;
    private bool _stopEnabled;
    private string _connectionStatus;
    private bool _connected;
    private bool _inProcess;

    private readonly DialogService _dialogService;
    private readonly SystemDialogService _systemDialogService;
    private readonly FilePickerService _filePickerService;
    private readonly ExpDeviceService _expDeviceService;
    private readonly MeasurementSettingsFormService _measurementSettingsFormService;
    private readonly ExpChartsData _expChartsData;

    public HeaderViewModel(
        DialogService dialogService,
        SystemDialogService systemDialogService,
        FilePickerService filePickerService,
        ExpDeviceService expDeviceService,
        MeasurementSettingsFormService measurementSettingsFormService,
        ExpChartsData expChartsData
    )
    {
        _dialogService = dialogService;
        _systemDialogService = systemDialogService;
        _filePickerService = filePickerService;
        _expDeviceService = expDeviceService;
        _measurementSettingsFormService = measurementSettingsFormService;
        _expChartsData = expChartsData;

        _expDeviceService.Connected
            .Subscribe(connected => { Connected = connected; });
        _expDeviceService.InProcess
            .Subscribe(inProcess => { InProcess = inProcess; });

        // Observable.Zip(_expDeviceService.InProcess, _expDeviceService.Connected)
        //     .Subscribe(result =>
        //     {
        //         Connected = result[1];
        //         InProcess = result[0];
        //     });

        this.WhenAnyValue(x => x.Connected, x => x.InProcess)
            .Subscribe(result =>
            {
                var (connected, inProcess) = result;

                PlayEnabled = connected && !inProcess;
                StopEnabled = connected && inProcess;

                Connected = connected;
                ConnectionStatus = GetUsbConnectionStatus(Connected);
            });

        OpenSettingsDialogCommand = ReactiveCommand.Create(OpenSettingsDialog);
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
        SaveCommand = ReactiveCommand.Create(Save);
    }

    private void OpenSettingsDialog()
    {
        _dialogService.Create<SettingsDialogViewModel>().Open();
    }

    private void ToggleActive()
    {
        if (!InProcess)
        {
            _measurementSettingsFormService.GetModelFromStorage()
                .Where(model => model != null)
                .Subscribe(model => { _expDeviceService.RunProcess(model.ToDto()); });

            return;
        }

        _expDeviceService.StopProcess();
    }

    public void Save()
    {
        string result = "";
        foreach (var item in _expChartsData.Data)
        {
            result += $"{item.Key}\n{string.Concat(item.Value.Select(value => $"{value[0]};{value[1]}\n").ToArray())}";
        }

        dynamic options = new
        {
            Title = "Сохранения значений всех графиков",
            FileName = "data",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("HTL")
                {
                    Patterns = new[] { "*.htl" },
                    AppleUniformTypeIdentifiers = new[] { "public.htl" },
                    MimeTypes = new[] { "text/htl" }
                }
            },
            DefaultExtension = "htl",
            StartLocationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        };
        _filePickerService.Save(result, (IFilePickerSaveOptions)options)
            .Subscribe();
    }

    private string GetUsbConnectionStatus(bool connected) =>
        connected ? "Устройство подключено" : "Устройство не подключено";
}