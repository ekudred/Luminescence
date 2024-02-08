using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Luminescence.Views;

namespace Luminescence;

// public delegate void MainHandler();

public partial class App : Application
{
    // private static MainHandler? mainwindowclosing;
    // public static void RegisterHandler(MainHandler del) => mainwindowclosing = del;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            // desktop.MainWindow.Closing += (s, e) => MainWindowClosing();
        }

        base.OnFrameworkInitializationCompleted();
    }

    // private void MainWindowClosing() => mainwindowclosing?.Invoke();
}