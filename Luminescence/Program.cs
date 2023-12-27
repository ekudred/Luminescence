using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using System;
using Splat;

namespace Luminescence;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        RegisterDependencies();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }

    private static void RegisterDependencies()
    {
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);
    }
}