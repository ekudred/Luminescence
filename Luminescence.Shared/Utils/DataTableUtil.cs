using System;
using System.Data;
using System.Text;

namespace Luminescence.Shared.Utils;

public static class DataTableUtil
{
    public static string ToText
    (
        this DataTable dataTable,
        string separator = ";",
        bool includeColumnName = true,
        bool trimColumValue = true,
        string defaultNullColumnValue = ""
    )
    {
        if (defaultNullColumnValue == null)
        {
            throw new NullReferenceException("'defaultNullColumnValue' should not be null");
        }

        StringBuilder result = new();

        if (includeColumnName)
        {
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                result.Append(dataTable.Columns[i]);

                if (i < dataTable.Columns.Count - 1)
                {
                    result.Append(separator);
                }
            }

            result.AppendLine();
        }

        long lineIndex = 0;
        foreach (DataRow row in dataTable.Rows)
        {
            lineIndex++;

            if (lineIndex > 1)
            {
                result.AppendLine();
            }

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var columnValue = row[i];
                var value = columnValue == DBNull.Value ? defaultNullColumnValue : columnValue.ToString()!;
                if (trimColumValue)
                {
                    value = value.Trim();
                }

                result.Append(value);
                if (i < dataTable.Columns.Count - 1)
                {
                    result.Append(separator);
                }
            }
        }

        return result.ToString();
    }
}