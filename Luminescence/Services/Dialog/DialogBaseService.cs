using System;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Luminescence.Dialog;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Services;

public class DialogBaseService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public DialogBaseService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IDialogWindow<TDialogViewModel> Create<TDialogViewModel>(Window? parentWindow = null)
        where TDialogViewModel : DialogBaseViewModel
    {
        string dialogViewModelName = typeof(TDialogViewModel).Name;

        DialogWindow<TDialogViewModel> dialog =
            CreateView<DialogWindow<TDialogViewModel>>(dialogViewModelName.Replace("ViewModel", string.Empty));
        TDialogViewModel dialogViewModel = CreateViewModel<TDialogViewModel>(dialogViewModelName);

        dialog.DataContext = dialogViewModel;
        dialog.ParentWindow = parentWindow ?? _mainWindowProvider.GetMainWindow();

        return dialog;
    }

    private static TView CreateView<TView>(string name)
    {
        var viewType = GetViewType(name);
        if (viewType is null)
        {
            throw new InvalidOperationException($"View for {name} was not found!");
        }

        return (TView)GetView(viewType)!;
    }

    private static Type? GetViewType(string name)
    {
        var viewsAssembly = Assembly.GetExecutingAssembly();
        var viewTypes = viewsAssembly.GetTypes();

        return viewTypes.SingleOrDefault(t => t.Name == name);
    }

    private static object? GetView(Type type)
    {
        return Activator.CreateInstance(type);
    }

    private static TViewModel CreateViewModel<TViewModel>(string name)
    {
        var viewModelType = GetViewModelType(name);
        if (viewModelType is null)
        {
            throw new InvalidOperationException($"View model {name} was not found!");
        }

        return (TViewModel)GetViewModel(viewModelType)!;
    }

    private static Type? GetViewModelType(string name)
    {
        var viewModelsAssembly = Assembly.GetAssembly(typeof(BaseViewModel));
        if (viewModelsAssembly is null)
        {
            throw new InvalidOperationException("Broken installation!");
        }

        var viewModelTypes = viewModelsAssembly.GetTypes();

        return viewModelTypes.SingleOrDefault(t => t.Name == name);
    }

    private static object? GetViewModel(Type type)
    {
        var service = Locator.Current.GetService(type);

        if (service == null)
        {
            return Activator.CreateInstance(type);
        }

        return service;
    }
}