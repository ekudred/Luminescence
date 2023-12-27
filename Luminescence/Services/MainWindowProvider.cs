using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Luminescence.Services;

public class MainWindowProvider
{
    public Window GetMainWindow()
    {
        Application current = Application.Current;
        IClassicDesktopStyleApplicationLifetime desktop = (IClassicDesktopStyleApplicationLifetime)current?.ApplicationLifetime;
        Window mainWindow = (Window)desktop?.MainWindow;

        return mainWindow ?? null;
    }
}