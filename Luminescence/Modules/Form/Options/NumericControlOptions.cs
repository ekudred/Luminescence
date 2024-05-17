namespace Luminescence.Form;

public class NumericControlOptions : FormControlOptions
{
    public string Placeholder = "";
    public NumericControlSpinnerOptions Spinner = new();
}

public class NumericControlSpinnerOptions
{
    public bool IsEnabled { get; set; } = true;
    public decimal Increment { get; set; } = new(0.05);
    public string FormatString { get; set; } = "0.00";
    public decimal Minimum { get; set; } = 0;
    public decimal Maximum { get; set; } = 1;
}