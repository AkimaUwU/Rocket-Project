using System.Text.RegularExpressions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.TimeRecognition;

public sealed record TimeRecognitionResult(long Seconds);

public static class TimeRecognizer
{
    private static readonly RegexOptions _options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
    private static readonly Regex[] _regexes =
    [
        new(@"в\s(\d{1,2} \d{2})\s", _options),
        new(@"в\s(\d{1,2}:\d{2})\s", _options),
        new(@"в\s(\d{1,2}:\d{2})", _options),
        new(@"в\s(\d{1,2} \d{2})", _options),
        new(@"\s(\d{1,2}:\d{2})\s", _options),
        new(@"\s(\d{1,2} \d{2})\s", _options),
    ];

    public static Option<TimeRecognitionResult> TryRecognize(string input)
    {
        Option<Match> match = input.GetMatch();
        if (!match.HasValue)
            return Option<TimeRecognitionResult>.None();

        string matchedString = match.Value.Groups[1].Value;
        return matchedString.Contains(':') switch
        {
            true => Option<TimeRecognitionResult>.Some(matchedString.ResultOneVer()),
            false => Option<TimeRecognitionResult>.Some(matchedString.ResultSecondVer()),
        };
    }

    private static Option<Match> GetMatch(this string input)
    {
        foreach (var regex in _regexes)
        {
            Match match = regex.Match(input);
            if (match.Success)
                return Option<Match>.Some(match);
        }

        return Option<Match>.None();
    }

    private static Option<int> ParseTime(this string time)
    {
        int timeValue = (time.Length == 1) ? int.Parse(time[0].ToString()) : 0;
        if (timeValue != 0)
            return Option<int>.Some(timeValue);

        timeValue = (time[0] == '0') ? int.Parse(time[^1].ToString()) : 0;
        if (timeValue != 0)
            return Option<int>.Some(timeValue);

        if (int.TryParse(time, out int parsedTimeValue))
            return Option<int>.Some(parsedTimeValue);
        return Option<int>.None();
    }

    private static (Option<int> parsedHours, Option<int> ParsedMinutes) ParseTimePair(
        this string[] time
    )
    {
        string hours = time[0];
        string minutes = time[1];
        Option<int> parsedHours = hours.ParseTime();
        Option<int> parsedMinutes = minutes.ParseTime();
        return (parsedHours, parsedMinutes);
    }

    private static Option<TimeRecognitionResult> ResultOneVer(this string matchedString)
    {
        string[] splittedPart = matchedString.Split(':');
        if (splittedPart.Length != 2)
            return Option<TimeRecognitionResult>.None();

        (Option<int> hours, Option<int> minutes) = splittedPart.ParseTimePair();
        if (!hours.HasValue || !minutes.HasValue)
            return Option<TimeRecognitionResult>.None();

        long totalSeconds = 3600 * hours.Value + 60 * minutes.Value;
        TimeRecognitionResult result = new(totalSeconds);
        return Option<TimeRecognitionResult>.Some(result);
    }

    private static Option<TimeRecognitionResult> ResultSecondVer(this string matchedString)
    {
        string[] splittedPart = matchedString.Split(' ');
        if (splittedPart.Length != 2)
            return Option<TimeRecognitionResult>.None();

        (Option<int> hours, Option<int> minutes) = splittedPart.ParseTimePair();
        if (!hours.HasValue || !minutes.HasValue)
            return Option<TimeRecognitionResult>.None();

        long totalSeconds = 3600 * hours.Value + 60 * minutes.Value;
        TimeRecognitionResult result = new(totalSeconds);
        return Option<TimeRecognitionResult>.Some(result);
    }
}
