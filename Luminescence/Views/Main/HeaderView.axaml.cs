using Avalonia.Controls;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class HeaderView : UserControl
{
    public HeaderView()
    {
        InitializeComponent();

        DataContext = Locator.Current.GetService<HeaderViewModel>();
    }
}