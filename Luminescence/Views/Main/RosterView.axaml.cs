using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class RosterView : UserControl
{
    public RosterView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = Locator.Current.GetService<RosterViewModel>();
    }
}