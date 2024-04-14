using System.Reactive.Subjects;
using Avalonia.Controls;

namespace Luminescence.Dialog;

public interface IDialogWindow<TDialogViewModel>
    where TDialogViewModel : DialogBaseViewModel
{
    public TDialogViewModel ViewModel { get; }
    public Window CurrentWindow { get; }
    public Window ParentWindow { get; set; }

    public Subject<object> OnOpen { get; }
    public Subject<object> OnClose { get; }

    public void Open();
    public void Close();
}