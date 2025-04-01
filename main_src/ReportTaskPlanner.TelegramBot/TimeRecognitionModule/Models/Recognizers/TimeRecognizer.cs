using System.Text.RegularExpressions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.Utils;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognizers;

public sealed partial class TimeRecognizer : ITimeRecognizer
{
    public async Task<TimeRecognition> TryRecognize(string input)
    {
        string formatted = input.CleanStringFromPrepositionsAndConjunctions();
        Option<SpecificTimeRecognition> fromSample1 = FromRegex_Sample_1(formatted);
        if (fromSample1.HasValue)
            return await Task.FromResult(fromSample1);
        Option<SpecificTimeRecognition> fromSample2 = FromRegex_Sample_2(formatted);
        if (fromSample2.HasValue)
            return await Task.FromResult(fromSample2.Value);
        Option<SpecificTimeRecognition> fromSample3 = FromRegex_Sample_3(formatted);
        if (fromSample3.HasValue)
            return await Task.FromResult(fromSample3.Value);
        return await Task.FromResult(new UnrecognizedTime());
    }

    private static Option<SpecificTimeRecognition> FromRegex_Sample_1(string input)
    {
        Option<string> matchedString = GetMatchedString(input, TimeRegex_Sample_1());
        if (!matchedString.HasValue)
            return Option<SpecificTimeRecognition>.None();
        string matchedStringValue = matchedString.Value;
        string formatted = matchedStringValue.KeepOnlyDigitsInString().RemoveExtraSpaces();
        string[] parts = formatted.Split(' ', StringSplitOptions.TrimEntries);
        int hour = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);
        return Option<SpecificTimeRecognition>.Some(new SpecificTimeRecognition(hour, minutes));
    }

    private static Option<SpecificTimeRecognition> FromRegex_Sample_2(string input)
    {
        Option<string> matchedString = GetMatchedString(input, TimeRegex_Sample_2());
        if (!matchedString.HasValue)
            return Option<SpecificTimeRecognition>.None();
        string matchedStringValue = matchedString.Value;
        string formatted = matchedStringValue.KeepOnlyDigitsInString().RemoveExtraSpaces();
        string[] parts = formatted.Split(' ', StringSplitOptions.TrimEntries);
        int hour = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);
        return Option<SpecificTimeRecognition>.Some(new SpecificTimeRecognition(hour, minutes));
    }

    private static Option<SpecificTimeRecognition> FromRegex_Sample_3(string input)
    {
        Option<string> matchedString = GetMatchedString(input, TimeRegex_Sample_3());
        if (!matchedString.HasValue)
            return Option<SpecificTimeRecognition>.None();
        string matchedStringValue = matchedString.Value;
        string formatted = matchedStringValue.KeepOnlyDigitsInString().RemoveExtraSpaces();
        string[] parts = formatted.Split(' ', StringSplitOptions.TrimEntries);
        int hour = int.Parse(parts[0]);
        return Option<SpecificTimeRecognition>.Some(new SpecificTimeRecognition(hour, 0));
    }

    private static Option<string> GetMatchedString(string input, Regex regex)
    {
        Match match = regex.Match(input);
        if (!match.Success || string.IsNullOrWhiteSpace(match.Groups[0].Value))
            return Option<string>.None();
        return Option<string>.Some(match.Groups[0].Value);
    }

    private const RegexOptions Options =
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant;

    [GeneratedRegex(@"(\b\d{1,2}\sч(?:асов|аса)?\s\d{1,2}\sм(?:мин|инут)?\b)", Options)]
    private static partial Regex TimeRegex_Sample_1();

    [GeneratedRegex(@"((\b\d{1,2}\s\d{1,2}\b))", Options)]
    private static partial Regex TimeRegex_Sample_2();

    [GeneratedRegex(@"(\b\d{1,2}\sч(?:асов|аса|\b))", Options)]
    private static partial Regex TimeRegex_Sample_3();

    // [GeneratedRegex(@"(\d{1,2}\sм(?:мин|инут)?)", Options)]
    // private static partial Regex TimeRegex_Sample_4();
}
