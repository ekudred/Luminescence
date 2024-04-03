using System;
using Avalonia;
using Luminescence.Dialog;
using Luminescence.ViewModels;
using ReactiveUI;

namespace Luminescence.Views;

public partial class ChartPanelDialog : DialogWindow<ChartPanelDialogViewModel>
{
    public ChartPanelDialog()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenAnyValue(view => view.Width, view => view.Height)
            .Subscribe(result =>
            {
                var (width, height) = result;

                if (ViewModel != null && ViewModel.ChartTabsViewModel != null)
                {
                    ViewModel.ChartTabsViewModel.Width = width;
                    ViewModel.ChartTabsViewModel.Height = height;
                }
            });
    }
}