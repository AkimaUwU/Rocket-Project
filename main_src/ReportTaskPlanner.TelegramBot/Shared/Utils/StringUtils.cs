using System.Text.RegularExpressions;

namespace ReportTaskPlanner.TelegramBot.Shared.Utils;

public static partial class StringUtils
{
    private static readonly string[] PrepositionsAndConjunctions =
    [
        "и",
        "в",
        "во",
        "не",
        "на",
        "с",
        "со",
        "к",
        "кто",
        "что",
        "как",
        "то",
        "из",
        "по",
        "у",
        "благодаря",
        "для",
        "а",
        "но",
        "если",
        "когда",
        "пока",
        "чтобы",
        "т. д.",
        "а",
        "или",
    ];

    private const RegexOptions Options =
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

    public static string CleanString(this string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? string.Empty
            : input
                .CleanFromPunctuation()
                .CleanFromExtraSpaces()
                .CleanFromNewLines()
                .CleanFromExtraSpaces();

    public static string CleanStringFromPrepositionsAndConjunctions(this string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? string.Empty
            : PrepositionsAndConjunctionsRegex.Replace(input, "").CleanString();

    public static string KeepOnlyDigitsInString(this string? input) =>
        string.IsNullOrWhiteSpace(input) ? string.Empty : OnlyDigitsRegex().Replace(input, " ");

    public static string RemoveExtraSpaces(this string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? string.Empty
            : ExtraSpacesCleanRegex2().Replace(input, " ").Trim();

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

    private static readonly string PrepositionsAndConjunctionsPattern =
        @"\b(" + string.Join("|", PrepositionsAndConjunctions) + @")\b";

    private static readonly Regex PrepositionsAndConjunctionsRegex = new Regex(
        PrepositionsAndConjunctionsPattern,
        Options
    );

    [GeneratedRegex(@"\D+", Options)]
    private static partial Regex OnlyDigitsRegex();

    [GeneratedRegex(@"\s+", Options)]
    private static partial Regex ExtraSpacesCleanRegex2();
}
