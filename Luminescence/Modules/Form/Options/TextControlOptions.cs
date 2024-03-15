using System.Text.RegularExpressions;

namespace Luminescence.Form;

public class TextControlOptions : FormControlOptions
{
    public string Placeholder = "";
    public Regex RegExMask = new(@"^\d+$");
}