using System;
using System.Threading.Tasks;
using Luminescence.Services;
using Luminescence.Views;

namespace Luminescence.Dialog;

public class DialogService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public DialogService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public async Task<TResult> ShowDialog<TResult, TParam>(DialogBase<TResult, TParam> dialogWindow, TParam param = null)
        where TResult : DialogBaseResult
        where TParam : DialogBaseParam
    {
        var mainWindow = (MainWindow)_mainWindowProvider.GetMainWindow();

        dialogWindow.ViewModel.OnInitialize(param);

        mainWindow.ShowOverlay();
        var result = await dialogWindow.ShowDialog<TResult>(mainWindow);
        mainWindow.HideOverlay();

        if (dialogWindow is IDisposable disposable)
        {
            disposable.Dispose();
        }

        return result;
    }
}