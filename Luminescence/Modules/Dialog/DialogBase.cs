using System;
using Avalonia.Controls;

namespace Luminescence.Dialog;

public class DialogBase<TResult, TParam> : Window
    where TResult : DialogBaseResult
    where TParam : DialogBaseParam
{
    public DialogBaseViewModel<TResult, TParam> ViewModel => (DialogBaseViewModel<TResult, TParam>)DataContext;

    private Window ParentWindow => (Window)Owner;

    protected DialogBase()
    {
        SubscribeToViewEvents();
    }

    protected virtual void OnOpened()
    {
    }

    private void OnOpened(object sender, EventArgs e)
    {
        LockSize();

        OnOpened();
    }

    private void LockSize()
    {
        MaxWidth = MinWidth = Width;
        MaxHeight = MinHeight = Height;
    }

    private void SubscribeToViewModelEvents() => ViewModel.CloseRequested += ViewModelOnCloseRequested;

    private void UnsubscribeFromViewModelEvents() => ViewModel.CloseRequested -= ViewModelOnCloseRequested;

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

    private void ViewModelOnCloseRequested(object sender, DialogResultEventArgs<TResult> args)
    {
        UnsubscribeFromViewModelEvents();
        UnsubscribeFromViewEvents();

        Close(args.Result);
    }
}

public class DialogBase : DialogBase<DialogBaseResult, DialogBaseParam>
{
}

public class DialogBase<TResult> : DialogBase<TResult, DialogBaseParam>
    where TResult : DialogBaseResult
{
}