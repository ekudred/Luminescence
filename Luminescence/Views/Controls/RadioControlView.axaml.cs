using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Luminescence.Views;

public partial class RadioControlView : UserControl
{
    public RadioControlView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}