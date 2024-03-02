using System;
using System.Linq;
using System.Reflection;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Utils;

public static class ViewModelUtil
{
    public static TViewModel CreateViewModel<TViewModel>(string viewModelName)
    {
        var viewModelType = GetViewModelType(viewModelName);
        if (viewModelType is null)
        {
            throw new InvalidOperationException($"View model {viewModelName} was not found!");
        }

        return (TViewModel)GetViewModel(viewModelType);
    }

    public static Type? GetViewModelType(string viewModelName)
    {
        var viewModelsAssembly = Assembly.GetAssembly(typeof(BaseViewModel));
        if (viewModelsAssembly is null)
        {
            throw new InvalidOperationException("Broken installation!");
        }

        var viewModelTypes = viewModelsAssembly.GetTypes();

        return viewModelTypes.SingleOrDefault(t => t.Name == viewModelName);
    }

    public static object GetViewModel(Type type)
    {
        return Locator.Current.GetService(type);
    }
}