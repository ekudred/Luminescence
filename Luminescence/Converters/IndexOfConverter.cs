using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Luminescence.Converters;

///    <MultiBinding Converter="{StaticResource IndexOfConverter}">
///        <Binding RelativeSource="{RelativeSource FindAncestor, AncestorType={x:Type ItemsControl}}" />
///        <Binding Path="." />
///    </MultiBinding>
public class IndexOfConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count != 2)
        {
            return BindingOperations.DoNothing;
        }

        if (values[0] is not ItemsControl || values[1] == null)
        {
            return BindingOperations.DoNothing;
        }

        var itemsControl = values[0] as ItemsControl;
        var item = values[1];
        var itemContainer = itemsControl.ContainerFromItem(item);

        if (itemContainer == null)
        {
            return BindingOperations.DoNothing;
        }

        var itemIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer);

        return itemIndex.ToString();
    }
}