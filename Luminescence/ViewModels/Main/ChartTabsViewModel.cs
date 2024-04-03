using System.Collections.Generic;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartTabsViewModel : BaseViewModel
{
    public Dictionary<string, ChartViewModel> Charts { get; set; }

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

    public string Test
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }

    private double _width;
    private double _height;
    private string _test;
}