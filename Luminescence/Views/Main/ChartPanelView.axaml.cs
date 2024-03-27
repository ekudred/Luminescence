using Avalonia.Controls;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class ChartPanelView : UserControl
{
    public ChartPanelView()
    {
        InitializeComponent();

        DataContext = Locator.Current.GetService<ChartPanelViewModel>();
    }
}