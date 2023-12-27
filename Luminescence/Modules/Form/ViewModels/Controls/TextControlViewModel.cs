namespace Luminescence.Form.ViewModels;

public class TextControlViewModel : FormControlBaseViewModel
{
    public TextControlViewModel(string name, string defaultValue = "", TextControlOptions options = null)
        : base(name)
    {
        Value = defaultValue == null ? "" : defaultValue;
        
        SetOptions(options);
    }

    private void SetOptions(TextControlOptions options)
    {
        if (options == null)
        {
            return;
        }
        
        base.SetOptions(options);
    }
}