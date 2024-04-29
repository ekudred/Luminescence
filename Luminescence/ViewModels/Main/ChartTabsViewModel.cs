using System.Collections.Generic;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartTabsViewModel : BaseViewModel
{
    public List<ChartTab> ChartTabs { get; }

    public double Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    public double Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }

    private double _width;
    private double _height;

    public ChartTabsViewModel(List<ChartTab> chartTabs)
    {
        ChartTabs = chartTabs;
    }
}