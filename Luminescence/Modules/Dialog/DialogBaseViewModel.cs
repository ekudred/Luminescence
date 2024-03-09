using System;
using System.Windows.Input;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Dialog;

public class DialogBaseViewModel : BaseViewModel
{
    public event EventHandler<object?> CloseRequested;

    public ICommand CloseCommand { get; }

    public bool CanClose = true;

    protected DialogBaseViewModel()
    {
        CloseCommand = ReactiveCommand.Create(Close);
    }

    public virtual void Initialize(object data)
    {
    }

    protected void Close()
    {
        CloseRequested.Invoke(this, null!);
    }
}