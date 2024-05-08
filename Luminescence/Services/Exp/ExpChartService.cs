using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Platform.Storage;
using Luminescence.Dialog;
using Luminescence.Utils;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class ExpChartService
{
    public readonly ExpChartsModel ExpChartsModel = new();

    public readonly BehaviorSubject<bool> OpenEnabled = new(true);
    public readonly BehaviorSubject<bool> SaveEnabled = new(false);

    private AppFilePickerOpenOptions _openOptions => new()
    {
        Title = "Получение графиков",
        FileTypeFilter = new[]
        {
            new FilePickerFileType("HTL")
            {
                Patterns = new[] { "*.htl" },
                AppleUniformTypeIdentifiers = new[] { "public.htl" },
                MimeTypes = new[] { "text/htl" }
            }
        },
        StartLocationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
    };

    private AppFilePickerSaveOptions _saveOptions => new()
    {
        Title = "Сохранение",
        FileName = $"Измерение {DateTime.Now.ToString("dd.MM.yyyy")}",
        FileTypeChoices = new[]
        {
            new FilePickerFileType("HTL")
            {
                Patterns = new[] { "*.htl" },
                AppleUniformTypeIdentifiers = new[] { "public.htl" },
                MimeTypes = new[] { "text/htl" }
            },
            // new FilePickerFileType("Excel")
            // {
            //     Patterns = new[] { "*.xlsx" },
            //     AppleUniformTypeIdentifiers = null,
            //     MimeTypes = null
            // }
        },
        DefaultExtension = "htl",
        StartLocationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
    };

    private readonly MeasurementSettingsFormViewModel _measurementSettingsFormViewModel;
    private readonly AppFilePickerService _appFilePickerService;
    private readonly ClipboardService _clipboardService;
    private readonly DialogService _dialogService;

    public ExpChartService(
        ExpDevice expDevice,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel,
        AppFilePickerService appFilePickerService,
        ClipboardService clipboardService,
        DialogService dialogService
    )
    {
        _measurementSettingsFormViewModel = measurementSettingsFormViewModel;
        _appFilePickerService = appFilePickerService;
        _clipboardService = clipboardService;
        _dialogService = dialogService;

        ExpChartsModel.Initialize();

        expDevice.InProcess
            .CombineWithPrevious()
            .Where(inProcess =>
                inProcess.Current && !inProcess.Previous && _measurementSettingsFormViewModel.ToModel().Clear)
            .Subscribe(_ => { ExpChartsModel.Clear(); });

        expDevice.InProcess
            .Subscribe(inProcess =>
            {
                OpenEnabled.OnNext(!inProcess);
                SaveEnabled.OnNext(!inProcess && ExpChartsModel.GetData().Result.Count > 0);
            });

        expDevice.CurrentData
            .Subscribe(data =>
            {
                ExpChartsModel.AddPoint(ExpChartsModel.Tal0, ExpChartsModel.CurrentSeriesName,
                    new double[] { data.Counter, data.Temperature });
                ExpChartsModel.AddPoint(ExpChartsModel.Tal0, ExpChartsModel.OpSeriesName,
                    new double[] { data.Counter, data.OpTemperature });

                ExpChartsModel.AddPoint(ExpChartsModel.Tal1, ExpChartsModel.CurrentSeriesName,
                    new double[] { data.Counter, data.Intensity });

                ExpChartsModel.AddPoint(ExpChartsModel.Tl, ExpChartsModel.CurrentSeriesName,
                    new double[] { data.OpTemperature, data.Intensity });

                ExpChartsModel.AddPoint(ExpChartsModel.Osl, ExpChartsModel.CurrentSeriesName,
                    new double[] { data.OpLEDCurrent, data.Intensity });

                ExpChartsModel.AddPoint(ExpChartsModel.Led, ExpChartsModel.CurrentSeriesName,
                    new double[] { data.Counter, data.LEDCurrent });
                ExpChartsModel.AddPoint(ExpChartsModel.Led, ExpChartsModel.OpSeriesName,
                    new double[] { data.Counter, data.OpLEDCurrent });
            });
    }

    public void Open()
    {
        if (!OpenEnabled.Value)
        {
            return;
        }

        _appFilePickerService.Open(_openOptions)
            .Select(text =>
            {
                return _dialogService.ShowConfirm(new ConfirmationDialogData("Открыть в новом окне?"))
                    .Select(confirm =>
                    {
                        if (!confirm)
                        {
                            return text;
                        }

                        var chartPanelDialog = _dialogService.Create<ChartPanelDialogViewModel>();

                        ExpChartsModel expChartsModel = new();
                        expChartsModel.Initialize();
                        expChartsModel.Fill(ExpChartsData.FromText((string)text));

                        chartPanelDialog.ViewModel.Initialize(new ChartPanelDialogData(new(expChartsModel.GetTabs())));

                        chartPanelDialog.Open();

                        return null;
                    });
            }).Switch()
            .Where(text => text != null)
            .Subscribe(text => { ExpChartsModel.Fill(ExpChartsData.FromText((string)text!)); });
    }

    public void Save()
    {
        if (!SaveEnabled.Value)
        {
            return;
        }

        string text = ExpChartsModel.GetData().ToText() + "\n" + _measurementSettingsFormViewModel.ToModel().ToString();

        _dialogService.ShowConfirm(new ConfirmationDialogData("Сохранить в буфер обмена?"))
            .Select(value => value
                ? _clipboardService.SetText(text)
                : _appFilePickerService.Save(text, _saveOptions)
            ).Switch()
            .Subscribe();
    }
}