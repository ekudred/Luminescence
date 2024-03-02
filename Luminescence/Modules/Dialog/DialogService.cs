using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Luminescence.Services;
using Luminescence.Utils;

namespace Luminescence.Dialog;

public class DialogService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public DialogService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IObservable<DialogBaseResult> ShowDialog(string dialogName)
    {
        return ShowDialog<DialogBaseResult, DialogBaseParam>(dialogName);
    }

    public IObservable<TResult> ShowDialog<TResult>(string dialogName)
        where TResult : DialogBaseResult
    {
        return ShowDialog<TResult, DialogBaseParam>(dialogName);
    }

    public IObservable<DialogBaseResult> ShowDialog<TParam>(string dialogName, TParam param)
        where TParam : DialogBaseParam
    {
        return ShowDialog<DialogBaseResult, TParam>(dialogName, param);
    }

    public IObservable<TResult> ShowDialog<TResult, TParam>(string dialogName, TParam? param = null)
        where TResult : DialogBaseResult
        where TParam : DialogBaseParam
    {
        return Observable.Create(async (IObserver<TResult> observer) =>
        {
            Window mainWindow = _mainWindowProvider.GetMainWindow();

            var dialog = ViewUtil.CreateView<DialogBase<TResult, TParam>>(dialogName);

            dialog.ViewModel.OnInitialize(param);

            TResult result = await dialog.ShowDialog<TResult>(mainWindow);

            if (dialog is IDisposable disposable)
            {
                disposable.Dispose();
            }

            observer.OnNext(result);
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }
}