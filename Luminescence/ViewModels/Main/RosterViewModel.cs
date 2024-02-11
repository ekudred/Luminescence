using System;
using Luminescence.Services;
using ReactiveUI;

namespace Luminescence.ViewModels;

public class RosterViewModel : BaseViewModel
{
    public bool Active
    {
        get => _active;
        private set => this.RaiseAndSetIfChanged(ref _active, value);
    }

    public RosterFormViewModel Form { get; }

    private bool _active;

    private readonly ExpUsbDeviceService _expDeviceUsbService;
    private readonly RosterFormService _rosterFormService;

    public RosterViewModel(
        ExpUsbDeviceService expDeviceUsbService,
        RosterFormService rosterFormService)
    {
        _expDeviceUsbService = expDeviceUsbService;
        _rosterFormService = rosterFormService;

        this.WhenAnyValue(x => x._expDeviceUsbService.Active)
            .Subscribe(active => { Active = active; });

        Form = new RosterFormViewModel();

        _rosterFormService.Initialize(Form);
    }
}