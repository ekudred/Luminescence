using Luminescence.Services;

namespace Luminescence.ViewModels;

public class RosterViewModel : BaseViewModel
{
    public RosterFormViewModel Form { get; }

    private readonly RosterFormService _rosterFormService;

    public RosterViewModel(RosterFormService rosterFormService)
    {
        _rosterFormService = rosterFormService;

        Form = new RosterFormViewModel();

        _rosterFormService.Initialize(Form);
    }
}