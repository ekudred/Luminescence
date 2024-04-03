using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia.Controls;
using Luminescence.Services;
using Luminescence.ViewModels;

namespace Luminescence.Dialog;

public class SystemDialogService : DialogService
{
    public SystemDialogService(MainWindowProvider mainWindowProvider) : base(mainWindowProvider)
    {
    }

    public IObservable<bool> Confirm(Window parentWindow)
    {
        return Confirm(null, parentWindow);
    }

    public IObservable<bool> Confirm(ConfirmationDialogData? data = null, Window? parentWindow = null)
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
                .Select(_ => Confirm(data, dialog.CurrentWindow)).Switch()
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