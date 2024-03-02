using System;
using System.Linq;
using System.Reflection;

namespace Luminescence.Utils;

public static class ViewUtil
{
    public static TView CreateView<TView>(string viewModelName)
    {
        var viewType = GetViewType(viewModelName);
        if (viewType is null)
        {
            throw new InvalidOperationException($"View for {viewModelName} was not found!");
        }

        return (TView)GetView(viewType)!;
    }

    public static Type? GetViewType(string viewModelName)
    {
        var viewsAssembly = Assembly.GetExecutingAssembly();
        var viewTypes = viewsAssembly.GetTypes();
        var viewName = viewModelName.Replace("ViewModel", string.Empty);

        return viewTypes.SingleOrDefault(t => t.Name == viewName);
    }

    public static object? GetView(Type type)
    {
        return Activator.CreateInstance(type);
    }
}