namespace Luminescence.Dialog;

public class ConfirmationDialogResult : DialogBaseResult
{
    public bool Confirmed { get; set; }

    public ConfirmationDialogResult(bool confirmed = true)
    {
        Confirmed = confirmed;
    }
}