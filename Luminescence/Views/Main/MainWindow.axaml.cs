using Avalonia;
using Avalonia.Controls;
using Luminescence.ViewModels;
using ReactiveUI;
using System;
using Avalonia.Input;
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

        DataContext = Locator.Current.GetService<MainWindowViewModel>();

        ViewModel.Initialize();

        this.WhenAnyValue(view => view.Width, view => view.Height)
            .Subscribe(result =>
            {
                var (width, height) = result;

                ViewModel.Width = width;
                ViewModel.Height = height;
            });
    }

    protected override void OnClosed(EventArgs args)
    {
        ViewModel.Destroy();

        base.OnClosed(args);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        Console.Write(args);
    }
}