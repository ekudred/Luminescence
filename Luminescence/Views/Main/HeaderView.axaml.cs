using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class HeaderView : UserControl
{
    public HeaderView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = Locator.Current.GetService<HeaderViewModel>();
    }
}