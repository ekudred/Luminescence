using System;
using System.Collections.Generic;
using Luminescence.Utils;

namespace Luminescence.ViewModels;

public class ChartTab
{
    public string Name { get; set; }
    public List<ChartViewModel> Charts { get; set; }

    public int Rows => Math.Ceiling((decimal)Charts.Count / Columns).ToInt();
    public int Columns => Charts.Count == 1 ? 1 : 2;
}