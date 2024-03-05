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

    public bool ApplyEnabled
    {
        get => _applyEnabled;
        set => this.RaiseAndSetIfChanged(ref _applyEnabled, value);
    }

    public ICommand ApplyCommand { get; }

    private bool _applyEnabled;

    private readonly ExpDeviceService _expDeviceService;

    public SettingsDialogViewModel(
        MeasurementSettingsFormService measurementSettingsFormService,
        ExpDeviceService expDeviceService
    )
    {
        _expDeviceService = expDeviceService;

        ApplyCommand = ReactiveCommand.Create(Apply);

        Form = new MeasurementSettingsFormViewModel();

        measurementSettingsFormService.Initialize(Form);
    }

    public override void OnInitialize(DialogBaseParam? param)
    {
        Form.FormChanged
            .TakeUntil(Form.destroyForm)
            .Subscribe(formChanged => { ApplyEnabled = formChanged; });
    }

    private void Apply()
    {
        Form.Apply();
    }
}