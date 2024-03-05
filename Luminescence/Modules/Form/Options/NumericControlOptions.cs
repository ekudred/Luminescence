namespace Luminescence.Form;

public class NumericControlOptions : FormControlOptions
{
    public NumericControlSpinnerOptions Spinner = new();
}

public class NumericControlSpinnerOptions
{
    public bool IsEnabled { get; set; } = false;
    public double Increment { get; set; } = 0.05;
    public string FormatString { get; set; } = "0.00";
    public double Minimum { get; set; } = 0;
    public double Maximum { get; set; } = 1;
}