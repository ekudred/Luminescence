using System.Reactive;
using Luminescence.Dialog;
using Luminescence.Services;
using Luminescence.Views;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class ToolBarViewModel : BaseViewModel
{
    public ReactiveCommand<Unit, Unit> OpenOptionsDialogCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleActiveCommand { get; }

    public string ConnectionStatus => _expDeviceUsbService.ConnectionStatus;
    public bool Active => _expDeviceUsbService.Initialized;

    private readonly DialogService _dialogService;
    private readonly ExpUsbDeviceService _expDeviceUsbService;

    public ToolBarViewModel(
        DialogService dialogService,
        ExpUsbDeviceService expDeviceUsbService
    )
    {
        _dialogService = dialogService;
        _expDeviceUsbService = expDeviceUsbService;

        OpenOptionsDialogCommand = ReactiveCommand.Create(OpenOptionsDialog);
        ToggleActiveCommand = ReactiveCommand.Create(ToggleActive);
    }

    public void ToggleActive()
    {
        if (!_expDeviceUsbService.Initialized)
        {
            _expDeviceUsbService.InitDevice();

            return;
        }
        
        _expDeviceUsbService.DestroyDevice();
    }

    public void OpenOptionsDialog()
    {
        _dialogService.ShowDialog(new OptionsDialog());
    }
}