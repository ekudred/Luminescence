namespace Luminescence.Dialog;

public class ErrorDialogData : DialogBaseData
{
    public readonly string Text;

    public ErrorDialogData(string text = "Произошла ошибка")
    {
        Text = text;
    }
}