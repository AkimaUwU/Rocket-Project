using System.Text.RegularExpressions;

namespace ReportTaskPlanner.TelegramBot.Shared.Utils;

public static partial class StringUtils
{
    private const RegexOptions Options =
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

    public static string CleanString(this string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? ""
            : input
                .CleanFromPunctuation()
                .CleanFromExtraSpaces()
                .CleanFromNewLines()
                .CleanFromExtraSpaces();

    public static int GetWordsCount(this string? input) =>
        string.IsNullOrWhiteSpace(input) ? 0 : input.Count(c => c == ' ') + 1;

    private static string CleanFromPunctuation(this string input)
    {
        string withoutPunctuation = PunctuationCleanRegex().Replace(input, " ");
        return withoutPunctuation;
    }

    private static string CleanFromExtraSpaces(this string input)
    {
        string withoutExtraSpaces = ExtraSpacesCleanRegex().Replace(input, " ").Trim();
        return withoutExtraSpaces;
    }

    private static string CleanFromNewLines(this string input)
    {
        string withoutNewLines = NewLineCleanRegex().Replace(input, " ").ReplaceLineEndings();
        return withoutNewLines;
    }

    [GeneratedRegex(@"(w*|s*)[,|.|;|:|!|?|`|@|#|$|%|^|&|*|(|)|-|+|\|](w*|s*)", Options)]
    private static partial Regex PunctuationCleanRegex();

    [GeneratedRegex(@"[\\r\\n]+", Options)]
    private static partial Regex NewLineCleanRegex();

    [GeneratedRegex(@"\s{2,}", Options)]
    private static partial Regex ExtraSpacesCleanRegex();
}
