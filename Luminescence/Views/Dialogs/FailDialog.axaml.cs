using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Luminescence.Dialog;
using Luminescence.Models;
using Luminescence.ViewModels;

namespace Luminescence.Views;

public partial class FailDialog : DialogBase
{
    public FailDialog()
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
        DataContext = new FailDialogViewModel(new FailModel(null));
        // DataContext = Locator.Current.GetService<FailDialogViewModel>();
    }
}