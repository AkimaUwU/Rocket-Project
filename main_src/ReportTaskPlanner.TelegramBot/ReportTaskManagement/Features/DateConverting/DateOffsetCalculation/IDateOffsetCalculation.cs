using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;

public interface IDateOffsetCalculation
{
    public Option<DateOffsetResult> Convert(string stringDate, ApplicationTime currentTime);
}
