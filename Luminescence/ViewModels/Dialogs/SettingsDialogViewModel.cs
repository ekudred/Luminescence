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

    public bool Loading
    {
        get => _loading;
        set => this.RaiseAndSetIfChanged(ref _loading, value);
    }

    public ICommand CancelCommand { get; }
    public ICommand ApplyCommand { get; }

    private bool _formChanged;
    private bool _loading;

    private readonly MeasurementSettingsFormService _measurementSettingsFormService;
    private readonly SystemDialogService _systemDialogService;

    public SettingsDialogViewModel(
        MeasurementSettingsFormService measurementSettingsFormService,
        MeasurementSettingsFormViewModel measurementSettingsFormViewModel,
        SystemDialogService systemDialogService
    )
    {
        _measurementSettingsFormService = measurementSettingsFormService;
        _systemDialogService = systemDialogService;

        Form = measurementSettingsFormViewModel;

        CancelCommand = ReactiveCommand.Create(Cancel);
        ApplyCommand = ReactiveCommand.Create(Apply);

        Form.FormChanged
            .TakeUntil(Form.destroyForm)
            .Subscribe(formChanged =>
            {
                CanClose = !formChanged;
                FormChanged = formChanged;
            });
    }

    public void UseConfirm(IDialogWindow<SettingsDialogViewModel> dialog)
    {
        _systemDialogService.UseConfirm(
                dialog,
                new ConfirmationDialogParam("Вы уверены, что не хотите применить изменения?")
            )
            .Subscribe(confirm =>
            {
                if (confirm)
                {
                    Cancel();
                }
            });
    }

    private void Apply()
    {
        Loading = true;

        _measurementSettingsFormService.SetModelToStorage(Form.ToModel())
            .Subscribe(_ =>
            {
                Form.Apply();

                Loading = false;
            });
    }

    private void Cancel()
    {
        Form.Cancel();
    }
}