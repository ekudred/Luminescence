using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Luminescence.Utils;

public static class ViewUtil
{
    public static TView CreateView<TView>(string name)
    {
        var viewType = GetViewType(name);
        if (viewType is null)
        {
            throw new InvalidOperationException($"View for {name} was not found!");
        }

        return (TView)GetView(viewType)!;
    }

    public static Type? GetViewType(string name)
    {
        var viewsAssembly = Assembly.GetExecutingAssembly();
        var viewTypes = viewsAssembly.GetTypes();

        return viewTypes.SingleOrDefault(t => t.Name == name);
    }

    public static object? GetView(Type type)
    {
        return Activator.CreateInstance(type);
    }
}