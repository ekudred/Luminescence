namespace Luminescence.Form;

public class NumericControlOptions : FormControlOptions
{
    public string Placeholder = "";
    public NumericControlSpinnerOptions Spinner = new();
}

public class NumericControlSpinnerOptions
{
    public bool IsEnabled { get; set; } = true;
    public bool ManualInputEnabled { get; set; } = true;
    public decimal Increment { get; set; } = 1;
    public string FormatString { get; set; } = "0.00";
    public decimal Minimum { get; set; } = decimal.MinValue;
    public decimal Maximum { get; set; } =  decimal.MaxValue;
}