using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.TimeRecognition;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting;

public sealed class WhenToNotifyTimeCalculator(DateOffsetResult date, TimeRecognitionResult time)
{
    private readonly DateOffsetResult _date = date;
    private readonly TimeRecognitionResult _time = time;

    public long CalculateWhenToFireTime()
    {
        long offsetDate = _date.Date.ToUnixTime();
        return offsetDate + _time.Seconds;
        // DateTime result = _date.Date.AddSeconds(_time.Seconds);
        // return result.ToUnixTime();
    }
}
