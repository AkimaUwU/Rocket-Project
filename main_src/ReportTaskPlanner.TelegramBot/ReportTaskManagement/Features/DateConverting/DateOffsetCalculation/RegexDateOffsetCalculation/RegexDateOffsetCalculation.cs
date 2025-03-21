using System.Text.RegularExpressions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation.RegexDateOffsetCalculation;

public abstract class RegexDateOffsetCalculation(Regex regex) : IDateOffsetCalculation
{
    protected readonly Regex _regex = regex;
    public abstract Option<DateOffsetResult> Convert(
        string stringDate,
        ApplicationTime currentTime
    );
}
