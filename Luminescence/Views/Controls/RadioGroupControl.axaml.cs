using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Luminescence.Views;

public partial class RadioGroupControl : UserControl
{
    public RadioGroupControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}