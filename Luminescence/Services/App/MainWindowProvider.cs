using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Luminescence.Services;

public class MainWindowProvider
{
    public Window GetMainWindow()
    {
        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        Window? mainWindow = desktop.MainWindow;

        if (mainWindow == null)
        {
            throw new Exception("MainWindow not found");
        }

        return mainWindow;
    }
}