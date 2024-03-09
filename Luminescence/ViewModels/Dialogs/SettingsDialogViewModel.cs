using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Luminescence.Dialog;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class SettingsDialogViewModel : DialogBaseViewModel
{
    public MeasurementSettingsFormViewModel Form { get; }

    public bool FormChanged
    {
        get => _formChanged;
        set => this.RaiseAndSetIfChanged(ref _formChanged, value);
    }

    public ICommand ApplyCommand { get; }
    public ICommand CancelCommand { get; }

    private bool _formChanged;

    private readonly ExpDeviceService _expDeviceService;
    private readonly DialogService _dialogService;

    public SettingsDialogViewModel(
        MeasurementSettingsFormService measurementSettingsFormService,
        ExpDeviceService expDeviceService,
        DialogService dialogService
    )
    {
        _expDeviceService = expDeviceService;
        _dialogService = dialogService;

        ApplyCommand = ReactiveCommand.Create(Apply);
        CancelCommand = ReactiveCommand.Create(Cancel);

        Form = new MeasurementSettingsFormViewModel();

        measurementSettingsFormService.Initialize(Form);

        Form.FormChanged
            .TakeUntil(Form.destroyForm)
            .Subscribe(formChanged =>
            {
                CanClose = !formChanged;
                FormChanged = formChanged;
            });
    }

    private void Apply()
    {
        Form.Apply();
    }

    private void Cancel()
    {
        // Form.Cancel();
    }
}