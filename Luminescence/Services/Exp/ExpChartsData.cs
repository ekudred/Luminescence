using System;
using System.Collections.Generic;
using System.Linq;
using Luminescence.Shared.Utils;

namespace Luminescence.Services;

public class ExpChartsData
{
    public static ExpChartsData FromText(string text)
    {
        string[] allRows = text.Replace("\r", "").Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        string[] series = allRows[0].Split(";");
        string[] columns = allRows[1].Split(";");

        string[] rows = new string[allRows.Length - 2];
        Array.Copy(allRows, 2, rows, 0, rows.Length);

        var data = new ExpChartsData();

        for (int i = 0; i < rows.Length; i++)
        {
            string[] row = rows[i].Split(";");

            for (var j = 0; j < row.Length; j++)
            {
                if (j % 2 != 0)
                {
                    continue;
                }

                string[] serie = series[j].Split("/");

                data.AddPoint(
                    serie[0],
                    serie[1],
                    columns[j],
                    columns[j + 1],
                    new[] { (double)row[j].ToDouble()!, (double)row[j + 1].ToDouble()! }
                );
            }
        }

        return data;
    }

    /// <summary>
    ///     key = chartName & seriesName & xAxisName & yAxisName
    ///     value = point[]
    /// </summary>
    public Dictionary<string, List<double[]>> Result { get; } = new();

    public void AddPoint(string chartName, string seriesName, string xAxisName, string yAxisName, double[] point)
    {
        string key = string.Join("&", chartName, seriesName, xAxisName, yAxisName);

        Result.TryGetValue(key, out List<double[]>? value);

        if (value == null)
        {
            Result.Add(key, new() { point });
        }
        else
        {
            value.Add(point);
        }
    }

    public string ToText()
    {
        var series = Result.Keys.Aggregate(
            new List<string>(),
            (acc, item) =>
            {
                string[] key = item.Split("&");

                acc.Add($"{key[0]}/{key[1]}");

                return acc;
            }
        );

        var columns = Result.Keys.Aggregate(
            new List<string>(),
            (acc, item) =>
            {
                string[] key = item.Split("&");

                acc.Add(key[2]);
                acc.Add(key[3]);

                return acc;
            }
        );

        double[][][] rows = new double[Result.Values.ElementAt(0).Count][][];

        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = new double[Result.Values.Count][];
            for (int j = 0; j < Result.Values.Count; j++)
            {
                if (i < Result.Values.ElementAt(j).Count)
                {
                    rows[i][j] = Result.Values.ElementAt(j).ElementAt(i);
                }
            }
        }

        string result = "";

        for (int i = 0; i < series.Count; i++)
        {
            string end = i == series.Count - 1 ? "\n" : ";";
            result += $"{series[i]};{series[i]}{end}";
        }

        for (int i = 0; i < columns.Count; i++)
        {
            string end = i == columns.Count - 1 ? "\n" : ";";
            result += $"{columns[i]}{end}";
        }

        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];

            for (int j = 0; j < row.Length; j++)
            {
                string end = j == row.Length - 1 ? i == rows.Length - 1 ? "" : "\n" : ";";
                result += $"{row[j][0]};{row[j][1]}{end}";
            }
        }

        return result;
    }
}