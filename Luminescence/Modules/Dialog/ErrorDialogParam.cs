namespace Luminescence.Dialog;

public class ErrorDialogParam : DialogBaseParam
{
    public readonly string Text;

    public ErrorDialogParam(string text = "Произошла ошибка")
    {
        Text = text;
    }
}