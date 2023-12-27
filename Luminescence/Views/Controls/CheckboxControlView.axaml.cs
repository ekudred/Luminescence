using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Luminescence.Views;

public partial class CheckboxControlView : UserControl
{
    public CheckboxControlView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}