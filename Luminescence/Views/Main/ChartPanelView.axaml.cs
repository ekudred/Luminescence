using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class ChartPanelView : UserControl
{
    public ChartPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        BindDataContext();
    }

    private void BindDataContext()
    {
        DataContext = Locator.Current.GetService<ChartPanelViewModel>();
    }
}