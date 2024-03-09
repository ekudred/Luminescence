using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Luminescence.Form.ViewModels;

public class RadioGroupControlViewModel : FormControlBaseViewModel
{
    public List<RadioControlViewModel> Items { get; }

    private string _groupId = Guid.NewGuid().ToString();

    public RadioGroupControlViewModel(
        string name,
        int defaultValue,
        List<RadioControlViewModel> items,
        RadioControlGroupOptions? options = null
    ) : base(name)
    {
        items.ForEach(item => { item.GroupId = _groupId; });
        Items = items;

        if (items.Count >= 0 && defaultValue <= items.Count - 1)
        {
            Items[defaultValue].Value = true;
            Value = Items[defaultValue];
        }

        SetOptions(options ?? new());

        // Items
        //     .Select(item => item.ValueChanges).Merge()
        //     .Select(_ => Items.Where(item => (bool)item.Value))
        //     .TakeUntil(destroyControl)
        //     .Subscribe(item =>
        //     {
        //         var value = item.ElementAt(0);
        //
        //         Value = value;
        //         ValueChanges.OnNext(item);
        //     });
    }

    private void SetOptions(RadioControlGroupOptions options)
    {
        base.SetOptions(options);
    }
}