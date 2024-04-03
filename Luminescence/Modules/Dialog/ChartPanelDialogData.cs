using Luminescence.ViewModels;

namespace Luminescence.Dialog;

public class ChartPanelDialogData
{
    public readonly ChartTabsViewModel ChartTabsViewModel;

    public ChartPanelDialogData(ChartTabsViewModel chartTabsViewModel)
    {
        ChartTabsViewModel = chartTabsViewModel;
    }
}