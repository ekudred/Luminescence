using Avalonia;
using Avalonia.Markup.Xaml;
using Luminescence.Dialog;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Views;

public partial class OptionsDialog : DialogBase
{
    public OptionsDialog()
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

    private void BindDataContext()
    {
        DataContext = Locator.Current.GetService<OptionsDialogViewModel>();
    }
}