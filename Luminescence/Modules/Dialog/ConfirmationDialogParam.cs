namespace Luminescence.Dialog;

public class ConfirmationDialogParam : DialogBaseParam
{
    public readonly string Text;

    public ConfirmationDialogParam(string text = "Вы уверены?")
    {
        Text = text;
    }
}