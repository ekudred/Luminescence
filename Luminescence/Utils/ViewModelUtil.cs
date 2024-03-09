using System;
using System.Linq;
using System.Reflection;
using Luminescence.ViewModels;
using Splat;

namespace Luminescence.Utils;

public static class ViewModelUtil
{
    public static TViewModel CreateViewModel<TViewModel>(string name)
    {
        var viewModelType = GetViewModelType(name);
        if (viewModelType is null)
        {
            throw new InvalidOperationException($"View model {name} was not found!");
        }

        return (TViewModel)GetViewModel(viewModelType)!;
    }

    public static Type? GetViewModelType(string name)
    {
        var viewModelsAssembly = Assembly.GetAssembly(typeof(BaseViewModel));
        if (viewModelsAssembly is null)
        {
            throw new InvalidOperationException("Broken installation!");
        }

        var viewModelTypes = viewModelsAssembly.GetTypes();

        return viewModelTypes.SingleOrDefault(t => t.Name == name);
    }

    public static object? GetViewModel(Type type)
    {
        var service = Locator.Current.GetService(type);

        if (service == null)
        {
            return Activator.CreateInstance(type);
        }

        return service;
    }
}