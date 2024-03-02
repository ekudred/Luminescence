using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Luminescence.Dialog;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class SettingsDialog : DialogBase
{
    public SettingsDialog()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        BindDataContext();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
    }

    private void BindDataContext()
    {
        DataContext = Locator.Current.GetService<SettingsDialogViewModel>();
    }
}