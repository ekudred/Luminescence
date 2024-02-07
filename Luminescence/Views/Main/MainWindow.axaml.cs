using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Luminescence.ViewModels;
using ReactiveUI;
using System;
using Splat;

namespace Luminescence.Views;

public partial class MainWindow : Window
{
    private Grid OverlayGrid => this.FindControl<Grid>("DialogOverlay");

    public MainWindow()
    {
        InitializeComponent();

        OnChangeSize();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    public void ShowOverlay() => OverlayGrid.ZIndex = 1000;

    public void HideOverlay() => OverlayGrid.ZIndex = -1;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        BindDataContext();
    }

    private void BindDataContext()
    {
        DataContext = Locator.Current.GetService<MainWindowViewModel>();
    }

    private void OnChangeSize()
    {
        MainWindowViewModel viewModel = Locator.Current.GetService<MainWindowViewModel>();

        this.WhenAnyValue(view => view.Width)
            .Subscribe(width => { viewModel.Width = width; });
        this.WhenAnyValue(view => view.Height)
            .Subscribe(height => { viewModel.Height = height; });
    }
}