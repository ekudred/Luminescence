using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Avalonia.Controls;

namespace Luminescence.Dialog;

public class DialogWindow<TDialogViewModel> : Window, IDialogWindow<TDialogViewModel>
    where TDialogViewModel : DialogBaseViewModel
{
    public TDialogViewModel ViewModel => (TDialogViewModel)DataContext!;
    public Window CurrentWindow => this;
    public Window ParentWindow { get; set; }

    public bool CanClose
    {
        get => ViewModel.CanClose;
        set => ViewModel.CanClose = value;
    }

    public Subject<object> OnOpen { get; } = new();
    public Subject<object> OnClose { get; } = new();

    protected override void OnOpened(EventArgs args)
    {
        Initialize();

        base.OnOpened(args);
    }

    protected override void OnClosed(EventArgs args)
    {
        Destroy();

        base.OnClosed(args);
    }

    protected override void OnClosing(CancelEventArgs args)
    {
        OnClose.OnNext(null!);

        args.Cancel = !CanClose;
        base.OnClosing(args);
    }

    protected virtual void Initialize()
    {
        ViewModel.CloseRequested += CloseRequested!;
    }

    protected virtual void Destroy()
    {
        ViewModel.CloseRequested -= CloseRequested!;
    }

    public void Open()
    {
        ShowDialog(ParentWindow).ToObservable()
            .Subscribe(_ => OnOpen.OnNext(true));
    }

    public new void Close()
    {
        // OnClose.OnNext(CanClose);
        //
        // if (!CanClose)
        // {
        //     return;
        // }

        base.Close();
    }

    private void CloseRequested(object sender, object? canClose)
    {
        Close();
    }
}