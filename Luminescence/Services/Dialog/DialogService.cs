using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls;
using Luminescence.Dialog;
using Luminescence.ViewModels;

namespace Luminescence.Services;

public class DialogService : DialogBaseService
{
    public DialogService(MainWindowProvider mainWindowProvider) : base(mainWindowProvider)
    {
    }

    public IObservable<Unit> ShowError()
    {
        return ShowError(null, null);
    }

    public IObservable<Unit> ShowError(Window parentWindow)
    {
        return ShowError(null, parentWindow);
    }

    public IObservable<Unit> ShowError(ErrorDialogData? data = null, Window? parentWindow = null)
    {
        return Observable.Create((IObserver<Unit> observer) =>
        {
            var dialog = Create<ErrorDialogViewModel>();

            dialog.ViewModel.Initialize(data ??= new());

            dialog.Open();

            observer.OnNext(Unit.Default);
            observer.OnCompleted();

            return Disposable.Empty;
        });
    }

    public IObservable<bool> ShowConfirm()
    {
        return ShowConfirm(null, null);
    }

    public IObservable<bool> ShowConfirm(Window parentWindow)
    {
        return ShowConfirm(null, parentWindow);
    }

    public IObservable<bool> ShowConfirm(ConfirmationDialogData? data = null, Window? parentWindow = null)
    {
        return Observable.Create((IObserver<bool> observer) =>
        {
            var dialog = Create<ConfirmationDialogViewModel>(parentWindow);

            dialog.ViewModel.Initialize(data ??= new());

            dialog.ViewModel.OnConfirm
                .Subscribe(confirm =>
                {
                    observer.OnNext(confirm);
                    observer.OnCompleted();
                    dialog.Close();
                });
            dialog.OnClose
                .Subscribe(_ =>
                {
                    observer.OnNext(false);
                    observer.OnCompleted();
                });

            dialog.Open();

            return Disposable.Empty;
        });
    }

    public IObservable<bool> UseConfirm<TDialogViewModel>
    (
        IDialogWindow<TDialogViewModel> dialog,
        ConfirmationDialogData? data = null,
        bool? trigger = null
    )
        where TDialogViewModel : DialogBaseViewModel
    {
        return Observable.Create((IObserver<bool> observer) =>
        {
            Subject<object> dialogClosed = new();

            dialog.ViewModel.CanClose = trigger ?? dialog.ViewModel.CanClose;

            dialog.OnClose
                .Where(_ => !dialog.ViewModel.CanClose)
                .Select(_ => ShowConfirm(data, dialog.CurrentWindow)).Switch()
                .TakeUntil(dialogClosed)
                .Subscribe(confirm =>
                {
                    dialog.ViewModel.CanClose = confirm;

                    if (dialog.ViewModel.CanClose)
                    {
                        dialog.Close();

                        dialogClosed.OnNext(default!);
                        dialogClosed.OnCompleted();
                        dialogClosed = null;
                    }

                    observer.OnNext(confirm);

                    if (dialogClosed == null)
                    {
                        observer.OnCompleted();
                    }
                });

            return Disposable.Empty;
        });
    }
}