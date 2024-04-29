using Luminescence.Dialog;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ChartPanelDialogViewModel : DialogBaseViewModel
{
    public ChartTabsViewModel ChartTabsViewModel
    {
        get => _chartTabsViewModel;
        set => this.RaiseAndSetIfChanged(ref _chartTabsViewModel, value);
    }

    private ChartTabsViewModel _chartTabsViewModel;

    public override void Initialize(object o)
    {
        var data = o as ChartPanelDialogData;

        ChartTabsViewModel = data.ChartTabsViewModel;
    }
}