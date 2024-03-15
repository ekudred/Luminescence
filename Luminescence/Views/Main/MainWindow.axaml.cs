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
    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = Locator.Current.GetService<MainWindowViewModel>();
        ViewModel.Initialize();

        OnChangeSize();
    }

    protected override void OnClosed(EventArgs args)
    {
        ViewModel.Destroy();

        base.OnClosed(args);
    }

    private void OnChangeSize()
    {
        MainWindowViewModel viewModel = Locator.Current.GetService<MainWindowViewModel>();

        this.WhenAnyValue(view => view.Width, view => view.Height)
            .Subscribe(result =>
            {
                var (width, height) = result;

                viewModel.Width = width;
                viewModel.Height = height;
            });
    }
}