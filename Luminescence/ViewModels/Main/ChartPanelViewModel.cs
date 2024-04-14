using System;
using Luminescence.Services;
using Newtonsoft.Json;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartPanelViewModel : BaseViewModel
{
    public ChartTabsViewModel ChartTabsViewModel { get; } = new();

    public string Test
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }

    private string _test;

    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly ExpChartService _expChartService;

    public ChartPanelViewModel(
        MainWindowViewModel mainWindowViewModel,
        ExpChartService expChartService,
        ExpDevice expDevice
    )
    {
        _mainWindowViewModel = mainWindowViewModel;
        _expChartService = expChartService;

        _expChartService.Initialize();

        OnChangeChartSizes();

        ChartTabsViewModel.Charts = _expChartService.ChartViewModels;

        expDevice.CurrentData
            .Subscribe(data =>
            {
                ChartTabsViewModel.Test = System.Text.Json.Nodes.JsonNode.Parse(JsonConvert.SerializeObject(data)).ToString();
            });
    }

    private void OnChangeChartSizes()
    {
        this.WhenAnyValue(viewModel => viewModel._mainWindowViewModel.Width)
            .Subscribe(width => { ChartTabsViewModel.Width = width; });
        this.WhenAnyValue(viewModel => viewModel._mainWindowViewModel.Height)
            .Subscribe(height => { ChartTabsViewModel.Height = height - 32; });
    }
}