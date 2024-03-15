using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Luminescence.Dialog;
using Luminescence.ViewModels;

namespace Luminescence.Views;

public partial class SettingsDialog : DialogWindow<SettingsDialogViewModel>
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
    }

    // protected override void Destroy()
    // {
    //     ViewModel.Form.Destroy();
    //
    //     base.Destroy();
    // }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key == Key.Escape)
        {
            Close();
        }
    }
}