using System.Text.RegularExpressions;

namespace ReportTaskPlanner.Utilities.Common;

public static class StringFormatter
{
    public static string RemoveExtraSpaces(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";
        return Regex.Replace(input.Trim(), @"\s+", " ");
    }
}
