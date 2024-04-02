using System.Collections.Generic;

namespace Luminescence.Services;

public class ExpChartsData
{
    public Dictionary<string, List<double[]>> Data { get; } = new();
}