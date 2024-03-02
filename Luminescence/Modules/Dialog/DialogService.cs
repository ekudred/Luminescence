using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using Avalonia.Threading;
using Luminescence.Services;
using Luminescence.ViewModels;
using Luminescence.Views;
using Splat;

namespace Luminescence.Dialog;

public class DialogService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public DialogService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IObservable<TResult> ShowDialog<TResult, TParam>
    (
        DialogBase<TResult, TParam> dialogWindow,
        TParam param = null
    )
        where TResult : DialogBaseResult
        where TParam : DialogBaseParam?
    {
        return Observable.Create(async (IObserver<TResult> observer) =>
        {
            MainWindow mainWindow = (MainWindow)_mainWindowProvider.GetMainWindow();

            // DialogBase<TResult, TParam> dialogWindow = CreateView(dialogWindowName);

            dialogWindow.ViewModel.OnInitialize(param);

            mainWindow.ShowOverlay();

            TResult result = await dialogWindow.ShowDialog<TResult>(mainWindow);

            mainWindow.HideOverlay();

            if (dialogWindow is IDisposable disposable)
            {
                disposable.Dispose();
            }

            observer.OnNext(result);
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }

    private static DialogBase<TResult> CreateView<TResult>(string viewModelName)
        where TResult : DialogBaseResult
    {
        var viewType = GetViewType(viewModelName);
        if (viewType is null)
        {
            throw new InvalidOperationException($"View for {viewModelName} was not found!");
        }

        return (DialogBase<TResult>)GetView(viewType);
    }

    private static Type? GetViewType(string viewModelName)
    {
        var viewsAssembly = Assembly.GetExecutingAssembly();
        var viewTypes = viewsAssembly.GetTypes();
        var viewName = viewModelName.Replace("ViewModel", string.Empty);

        return viewTypes.SingleOrDefault(t => t.Name == viewName);
    }

    private static object GetView(Type type) => Activator.CreateInstance(type);

    private static DialogBaseViewModel<TResult> CreateViewModel<TResult>(string viewModelName)
        where TResult : DialogBaseResult
    {
        var viewModelType = GetViewModelType(viewModelName);
        if (viewModelType is null)
        {
            throw new InvalidOperationException($"View model {viewModelName} was not found!");
        }

        return (DialogBaseViewModel<TResult>) GetViewModel(viewModelType);
    }

    private static Type? GetViewModelType(string viewModelName)
    {
        var viewModelsAssembly = Assembly.GetAssembly(typeof(BaseViewModel));
        if (viewModelsAssembly is null)
        {
            throw new InvalidOperationException("Broken installation!");
        }

        var viewModelTypes = viewModelsAssembly.GetTypes();

        return viewModelTypes.SingleOrDefault(t => t.Name == viewModelName);
    }

    private static object GetViewModel(Type type) => Locator.Current.GetService(type);
}