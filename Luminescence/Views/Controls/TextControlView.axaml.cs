using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Luminescence.Views;

public partial class TextControlView : UserControl
{
    public TextControlView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}