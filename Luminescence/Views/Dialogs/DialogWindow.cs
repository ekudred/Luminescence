using System;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;

namespace Luminescence.Dialog;

public class DialogWindow<TDialogViewModel> : Window, IDialogWindow<TDialogViewModel>
    where TDialogViewModel : DialogBaseViewModel
{
    public TDialogViewModel ViewModel => (TDialogViewModel)DataContext!;
    public Window CurrentWindow => this;
    public Window ParentWindow { get; set; }

    public Subject<object> OnOpen { get; } = new();
    public Subject<object> OnClose { get; } = new();

    public void Open()
    {
        ShowDialog(ParentWindow).ToObservable()
            .Subscribe(_ => OnOpen.OnNext(default!));
    }

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

    protected override void OnClosing(WindowClosingEventArgs args)
    {
        OnClose.OnNext(default!);

        args.Cancel = !ViewModel.CanClose;
        base.OnClosing(args);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key == Key.Escape)
        {
            Close();
        }
    }

    protected virtual void Initialize()
    {
        ViewModel.CloseRequested += CloseRequested!;
    }

    protected virtual void Destroy()
    {
        ViewModel.CloseRequested -= CloseRequested!;
    }

    private void CloseRequested(object sender, object? canClose)
    {
        Close();
    }
}