using Avalonia.Controls;
using Avalonia.Input;

namespace Luminescence.Shared.Utils;

public static class TextInputUtil
{
    public static string GetValueFromEventArgs(TextInputEventArgs args)
    {
        TextBox control = (TextBox)args.Source!;
        string text = control.Text ?? string.Empty;

        return text.Substring(0, control.CaretIndex) + args.Text + text.Substring(control.CaretIndex);
    }
}