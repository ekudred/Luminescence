using System;
using Avalonia.Controls;

namespace Luminescence.Dialog;

public class DialogBase<TResult, TParam> : Window
    where TResult : DialogBaseResult
    where TParam : DialogBaseParam
{
    public DialogBaseViewModel<TResult, TParam> ViewModel => (DialogBaseViewModel<TResult, TParam>)DataContext;

    protected DialogBase()
    {
        SubscribeToViewEvents();
    }

    private void SubscribeToViewModelEvents() => ViewModel.CloseRequested += OnCloseRequested;

    private void UnsubscribeFromViewModelEvents() => ViewModel.CloseRequested -= OnCloseRequested;

    private void SubscribeToViewEvents()
    {
        DataContextChanged += OnDataContextChanged;
        Opened += OnOpened;
    }

    private void UnsubscribeFromViewEvents()
    {
        DataContextChanged -= OnDataContextChanged;
        Opened -= OnOpened;
    }

    private void OnDataContextChanged(object sender, EventArgs e) => SubscribeToViewModelEvents();

    private void OnCloseRequested(object sender, DialogResultEventArgs<TResult> args)
    {
        UnsubscribeFromViewModelEvents();
        UnsubscribeFromViewEvents();

        Close(args.Result);
    }

    private void OnOpened(object sender, EventArgs e)
    {
        LockSize();
    }

    private void LockSize()
    {
        MaxWidth = MinWidth = Width;
        MaxHeight = MinHeight = Height;
    }
}

public class DialogBase : DialogBase<DialogBaseResult, DialogBaseParam>
{
}

public class DialogBase<TResult> : DialogBase<TResult, DialogBaseParam>
    where TResult : DialogBaseResult
{
}