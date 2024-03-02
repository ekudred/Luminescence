using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Luminescence.Form.ViewModels;

public class RadioGroupControlViewModel : FormControlBaseViewModel
{
    public new readonly Subject<RadioControlViewModel> ValueChanges = new();

    public new RadioControlViewModel Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);

            ValueChanges.OnNext(value);
        }
    }

    public ObservableCollection<RadioControlViewModel> Items { get; set; }

    private RadioControlViewModel _value;

    private readonly string _groupId = Guid.NewGuid().ToString();

    public RadioGroupControlViewModel(
        string name,
        int defaultValue,
        List<RadioControlViewModel> items,
        RadioControlGroupOptions? options = null
    ) : base(name)
    {
        items.ForEach(item => { item.GroupId = _groupId; });
        Items = new ObservableCollection<RadioControlViewModel>(items);

        if (items.Count >= 0 && defaultValue <= items.Count - 1)
        {
            Items[defaultValue].Value = true;
            Value = Items[defaultValue];
        }

        SetOptions(options);

        Items
            .Select(item => item.ValueChanges)
            .Merge()
            .Select(_ => Items.Where(item => item.Value).ElementAt(0))
            .Subscribe(item =>
            {
                Value = item;
                ValueChanges.OnNext(item);
            });
    }

    private void SetOptions(RadioControlGroupOptions options)
    {
        if (options == null)
        {
            return;
        }

        base.SetOptions(options);
    }
}