using System;
using System.Windows.Input;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Dialog;

public class DialogBaseViewModel<TResult, TParam> : BaseViewModel
    where TResult : DialogBaseResult
    where TParam : DialogBaseParam
{
    public event EventHandler<DialogResultEventArgs<TResult>> CloseRequested;

    public ICommand CloseCommand { get; }

    protected DialogBaseViewModel()
    {
        CloseCommand = ReactiveCommand.Create(Close);
    }

    public virtual void OnInitialize(TParam param)
    {
    }

    protected void Close() => Close(default);

    protected void Close(TResult result)
    {
        var args = new DialogResultEventArgs<TResult>(result);

        CloseRequested.Invoke(this, args);
    }
}

public class DialogBaseViewModel : DialogBaseViewModel<DialogBaseResult, DialogBaseParam>
{
}

public class DialogBaseViewModel<TResult> : DialogBaseViewModel<TResult, DialogBaseParam>
    where TResult : DialogBaseResult
{
}