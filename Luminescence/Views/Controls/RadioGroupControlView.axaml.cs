using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Luminescence.Views;

public partial class RadioGroupControlView : UserControl
{
    public RadioGroupControlView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}