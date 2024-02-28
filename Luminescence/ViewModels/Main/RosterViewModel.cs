using System;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class RosterViewModel : BaseViewModel
{
    public bool InProcess
    {
        get => _inProcess;
        private set => this.RaiseAndSetIfChanged(ref _inProcess, value);
    }

    public RosterFormViewModel Form { get; }

    private bool _inProcess;

    private readonly ExpDeviceService _expDeviceService;
    private readonly RosterFormService _rosterFormService;

    public RosterViewModel(
        ExpDeviceService expDeviceService,
        RosterFormService rosterFormService)
    {
        _expDeviceService = expDeviceService;
        _rosterFormService = rosterFormService;

        _expDeviceService.InProcess
            .Subscribe(inProcess => { InProcess = inProcess; });

        Form = new RosterFormViewModel();

        _rosterFormService.Initialize(Form);
    }
}