using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

public abstract record TimeRecognitionTicket;

public sealed record PeriodicTimeRecognitionTicket(string Message) : TimeRecognitionTicket;

public sealed record SingleTimeRecognitionTicket(string Message) : TimeRecognitionTicket;

public sealed record UnknownRecognitionTicket : TimeRecognitionTicket;

public static class TimeRecognitionTicketModifiers
{
    public static ReportTaskSchedule ApplyModification(
        this TimeRecognitionTicket ticket,
        ReportTaskSchedule schedule
    ) =>
        ticket switch
        {
            PeriodicTimeRecognitionTicket => schedule.ApplyPeriodicTimeSchedule(),
            _ => schedule,
        };

    private static ReportTaskSchedule ApplyPeriodicTimeSchedule(this ReportTaskSchedule schedule) =>
        new ReportTaskSchedule(schedule, true);
}
