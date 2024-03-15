using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class RadioGroupControlViewModel : FormControlBaseViewModel
{
    public List<RadioControlViewModel> Items { get; }

    private string _groupId = Guid.NewGuid().ToString();

    public RadioGroupControlViewModel(
        string name,
        string defaultValue,
        List<RadioControlViewModel> items,
        RadioControlGroupOptions? options = null
    ) : base(name)
    {
        items.ForEach(item => { item.GroupId = _groupId; });
        Items = items;

        RadioControlViewModel defaultItem = Items.Find(item => item.Name == defaultValue);

        if (defaultItem != null)
        {
            defaultItem.Value = true;
            Value = defaultItem.Name;
        }

        SetOptions(options ?? new());

        Items
            .Select(item => item.ValueChanges).Merge()
            .Select(_ => Items.Where(item => item.Value is bool ? (bool)item.Value : false))
            .Select(item => item.ElementAt(0))
            .TakeUntil(destroyControl)
            .Subscribe(item =>
            {
                Value = item.Name;
                ValueChanges.OnNext(Value);
            });

        this.WhenAnyValue(x => x.Value)
            .TakeUntil(destroyControl)
            .Subscribe(value => { Items.ForEach(item => { item.Value = item.Name == (string)value; }); });
    }

    private void SetOptions(RadioControlGroupOptions options)
    {
        base.SetOptions(options);
    }
}