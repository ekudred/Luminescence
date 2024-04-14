namespace Luminescence.Dialog;

public class ConfirmationDialogData : DialogBaseData
{
    public readonly string Text;

    public ConfirmationDialogData(string text = "Вы уверены?")
    {
        Text = text;
    }
}