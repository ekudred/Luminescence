namespace Luminescence.Form.ViewModels;

public class CheckboxControlViewModel : FormControlBaseViewModel
{
    public CheckboxControlViewModel(string name, bool defaultValue = false, CheckboxControlOptions? options = null)
        : base(name)
    {
        Value = defaultValue;

        SetOptions(options ?? new());
    }

    private void SetOptions(CheckboxControlOptions options)
    {
        base.SetOptions(options);
    }
}