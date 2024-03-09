using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

    public IObservable<bool> Confirm(ConfirmationDialogParam? data = null, Window? parentWindow = null)
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

    public void UseConfirm<TDialogViewModel>(IDialogWindow<TDialogViewModel> dialog, bool? trigger = null)
        where TDialogViewModel : DialogBaseViewModel
    {
        dialog.CanClose = trigger ?? dialog.CanClose;
        dialog.OnClose
            .Where(_ => !dialog.CanClose)
            .Select(_ => Confirm(dialog.CurrentWindow)).Switch()
            .Subscribe(confirm =>
            {
                dialog.CanClose = confirm;

                if (dialog.CanClose)
                {
                    dialog.Close();
                }
            });
    }
}