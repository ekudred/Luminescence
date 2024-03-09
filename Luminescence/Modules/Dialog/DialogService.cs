using Avalonia.Controls;
using Luminescence.Services;
using Luminescence.Utils;

namespace Luminescence.Dialog;

public class DialogService
{
    private readonly MainWindowProvider _mainWindowProvider;

    public DialogService(MainWindowProvider mainWindowProvider)
    {
        _mainWindowProvider = mainWindowProvider;
    }

    public IDialogWindow<TDialogViewModel> Create<TDialogViewModel>(Window? parentWindow = null)
        where TDialogViewModel : DialogBaseViewModel
    {
        string dialogViewModelName = typeof(TDialogViewModel).Name;

        DialogWindow<TDialogViewModel> dialog = ViewUtil
            .CreateView<DialogWindow<TDialogViewModel>>(dialogViewModelName.Replace("ViewModel", string.Empty));
        TDialogViewModel dialogViewModel = ViewModelUtil
            .CreateViewModel<TDialogViewModel>(dialogViewModelName);

        dialog.DataContext = dialogViewModel;
        dialog.ParentWindow = parentWindow ?? _mainWindowProvider.GetMainWindow();

        return dialog;
    }
}