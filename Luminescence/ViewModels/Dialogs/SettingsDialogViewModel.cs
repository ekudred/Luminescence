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

    public SettingsDialogViewModel(MeasurementSettingsFormService measurementSettingsFormService)
    {
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
        var a = Form.ToModel();

        Console.Write(a);
    }
}