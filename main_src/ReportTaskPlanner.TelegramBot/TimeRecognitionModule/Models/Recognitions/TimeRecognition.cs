using System.Diagnostics;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

public interface IApplicationTimeModifier
{
    public ApplicationTime Modify(ApplicationTime time);
}

public interface IApplicationTimeCalculation : IApplicationTimeModifier
{
    public ApplicationTime Calculate(ApplicationTime calculation);
}

public abstract record TimeRecognition : IApplicationTimeCalculation
{
    public abstract ApplicationTime Calculate(ApplicationTime calculation);

    public abstract ApplicationTime Modify(ApplicationTime time);
}

public record UnrecognizedTime : TimeRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation) => calculation;

    public override ApplicationTime Modify(ApplicationTime time) => time;
}

public sealed record DayOfWeekRecognition(string DayOfWeek) : TimeRecognition
{
    public static readonly DayOfWeekRecognition Monday = new("Понедельник");
    public static readonly DayOfWeekRecognition Tuesday = new("Вторник");
    public static readonly DayOfWeekRecognition Wednesday = new("Среда");
    public static readonly DayOfWeekRecognition Thursday = new("Четверг");
    public static readonly DayOfWeekRecognition Friday = new("Пятница");
    public static readonly DayOfWeekRecognition Saturday = new("Суббота");
    public static readonly DayOfWeekRecognition Sunday = new("Воскресенье");

    public override ApplicationTime Calculate(ApplicationTime calculation)
    {
        int dayOfWeekNumber = GetDayOfWeekNumber();
        int currentDayOfWeek = (int)calculation.DateTime.DayOfWeek;
        int daysUntilRequested = dayOfWeekNumber - currentDayOfWeek;
        if (daysUntilRequested < 0)
            daysUntilRequested += 7;
        DateTime dateWithAddedDays = calculation.DateTime.Date.AddDays(daysUntilRequested);
        return new ApplicationTime(calculation, dateWithAddedDays);
    }

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);

    public int GetDayOfWeekNumber()
    {
        return DayOfWeek switch
        {
            "Понедельник" => 1,
            "Вторник" => 2,
            "Среда" => 3,
            "Четверг" => 4,
            "Пятница" => 5,
            "Суббота" => 6,
            "Воскресенье" => 0,
            _ => throw new UnreachableException(),
        };
    }
}

public sealed record MonthRecognition(int Month, int MonthDay) : TimeRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation)
    {
        DateTime currentDate = calculation.DateTime;
        int year = currentDate.Year;
        DateTime newDate = new(year, Month, MonthDay);
        if (newDate < currentDate)
            newDate = newDate.AddYears(1);
        return new ApplicationTime(calculation, newDate);
    }

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}

public abstract record PeriodicRecognition : TimeRecognition;

public sealed record PeriodicWeekDayRecognition(DayOfWeekRecognition DayOfWeek)
    : PeriodicRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation)
    {
        return DayOfWeek.Calculate(calculation);
    }

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}

public sealed record PeriodicEveryDayRecognition(RelativeRecognition Relative) : PeriodicRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation) =>
        Relative.Calculate(calculation);

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}

public sealed record PeriodicEveryHourRecognition(SpecificTimeRecognition Specific)
    : PeriodicRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation) =>
        Specific.Calculate(calculation);

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}

public sealed record PeriodicEveryMinuteRecognition(SpecificTimeRecognition Specific)
    : PeriodicRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation) =>
        Specific.Calculate(calculation);

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}

public sealed record RelativeRecognition(int DaysOffset) : TimeRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation)
    {
        DateTime calculated = calculation.DateTime.Date.AddDays(DaysOffset);
        return new ApplicationTime(calculation, calculated);
    }

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}

public sealed record SpecificTimeRecognition(int Hours, int Minutes) : TimeRecognition
{
    public override ApplicationTime Calculate(ApplicationTime calculation)
    {
        long seconds = 3600 * Hours + 60 * Minutes;
        return new ApplicationTime(calculation, seconds);
    }

    public override ApplicationTime Modify(ApplicationTime time) => Calculate(time);
}
