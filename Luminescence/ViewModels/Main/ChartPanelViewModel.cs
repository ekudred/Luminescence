using System;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartPanelViewModel : BaseViewModel
{
    public ChartTabsViewModel ChartTabsViewModel { get; }

    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly ExpChartService _expChartService;

    public ChartPanelViewModel(
        MainWindowViewModel mainWindowViewModel,
        ExpChartService expChartService
    )
    {
        _mainWindowViewModel = mainWindowViewModel;
        _expChartService = expChartService;

        ChartTabsViewModel = new(_expChartService.ExpChartsModel.GetTabs());

        this.WhenAnyValue(viewModel => viewModel._mainWindowViewModel.Width)
            .Subscribe(width => { ChartTabsViewModel.Width = width; });
        this.WhenAnyValue(viewModel => viewModel._mainWindowViewModel.Height)
            .Subscribe(height => { ChartTabsViewModel.Height = height - 32; });
    }
}