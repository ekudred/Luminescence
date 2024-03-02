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
    public MainWindow()
    {
        InitializeComponent();

        OnChangeSize();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        BindDataContext();

        ((MainWindowViewModel)DataContext).Initialize();

        Closing += (_, _) => ((MainWindowViewModel)DataContext).Destroy();
    }

    private void BindDataContext()
    {
        DataContext = Locator.Current.GetService<MainWindowViewModel>();
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