using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.TimeRecognition;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace TgBotPlannerTests;

public sealed class ParseTimeTests
{
    private IDateOffsetCalculation CreateCompositeConverter()
    {
        CompositeDateConverter converter = new CompositeDateConverter();
        converter = converter
            .With(new RelativeDateRegexConverter())
            .With(new DayOfWeekRegexConverter())
            .With(new MonthDateRegexConverter());
        return converter;
    }

    private Option<TimeRecognitionResult> TryRecognizeTime(string input) =>
        TimeRecognizer.TryRecognize(input);

    private WhenToNotifyTimeCalculator CreateCalculator(
        TimeRecognitionResult time,
        DateOffsetResult date
    ) => new(date, time);

    [Fact]
    public void Relative_Date_Time_Tomorrow_Calculation_Test()
    {
        string input = "завтра в 18:36 назначить генеральную уборку.";
        DateTime date = DateTime.Now;
        long unixTimeStamp = date.ToUnixTime();
        ApplicationTime time = new("Test Zone", "Test Name", unixTimeStamp, date);

        Option<TimeRecognitionResult> timeRecognizion = TimeRecognizer.TryRecognize(input);
        Assert.True(timeRecognizion.HasValue);

        IDateOffsetCalculation composite = CreateCompositeConverter();
        Option<DateOffsetResult> dateOffset = composite.Convert(input, time);
        Assert.True(dateOffset.HasValue);

        WhenToNotifyTimeCalculator calculator = CreateCalculator(timeRecognizion, dateOffset);
        long unixTime = calculator.CalculateWhenToFireTime();
        DateTime newDate = unixTime.FromUnixTime();
        Assert.Equal(date.Day + 1, newDate.Day);
        Assert.Equal(date.Month, newDate.Month);
        Assert.Equal(date.Year, newDate.Year);
        Assert.Equal(18, newDate.Hour);
        Assert.Equal(36, newDate.Minute);
    }

    [Fact]
    public void Relative_Date_Time_Today_Calculation_Test()
    {
        string input = "Сегодня в 18:36 назначить генеральную уборку.";
        DateTime date = DateTime.Now;
        long unixTimeStamp = date.ToUnixTime();
        ApplicationTime time = new("Test Zone", "Test Name", unixTimeStamp, date);

        Option<TimeRecognitionResult> timeRecognizion = TimeRecognizer.TryRecognize(input);
        Assert.True(timeRecognizion.HasValue);

        IDateOffsetCalculation composite = CreateCompositeConverter();
        Option<DateOffsetResult> dateOffset = composite.Convert(input, time);
        Assert.True(dateOffset.HasValue);

        WhenToNotifyTimeCalculator calculator = CreateCalculator(timeRecognizion, dateOffset);
        long unixTime = calculator.CalculateWhenToFireTime();
        DateTime newDate = unixTime.FromUnixTime();
        Assert.Equal(date.Day, newDate.Day);
        Assert.Equal(date.Month, newDate.Month);
        Assert.Equal(date.Year, newDate.Year);
        Assert.Equal(18, newDate.Hour);
        Assert.Equal(36, newDate.Minute);
    }

    [Fact]
    public void Relative_Date_Time_After_Tomorrow_Calculation_Test()
    {
        string input = "Послезавтра в 18:36 назначить генеральную уборку.";
        DateTime date = DateTime.Now;
        long unixTimeStamp = date.ToUnixTime();
        ApplicationTime time = new("Test Zone", "Test Name", unixTimeStamp, date);

        Option<TimeRecognitionResult> timeRecognizion = TimeRecognizer.TryRecognize(input);
        Assert.True(timeRecognizion.HasValue);

        IDateOffsetCalculation composite = CreateCompositeConverter();
        Option<DateOffsetResult> dateOffset = composite.Convert(input, time);
        Assert.True(dateOffset.HasValue);

        WhenToNotifyTimeCalculator calculator = CreateCalculator(timeRecognizion, dateOffset);
        long unixTime = calculator.CalculateWhenToFireTime();
        DateTime newDate = unixTime.FromUnixTime();
        Assert.Equal(date.Day + 2, newDate.Day);
        Assert.Equal(date.Month, newDate.Month);
        Assert.Equal(date.Year, newDate.Year);
        Assert.Equal(18, newDate.Hour);
        Assert.Equal(36, newDate.Minute);
    }

    [Fact]
    public void Week_Date_Time1_Calculation_Test()
    {
        string input = "В субботу в 18:36 назначить генеральную уборку.";
        DateTime date = DateTime.Now;
        long unixTimeStamp = date.ToUnixTime();
        ApplicationTime time = new("Test Zone", "Test Name", unixTimeStamp, date);

        Option<TimeRecognitionResult> timeRecognizion = TimeRecognizer.TryRecognize(input);
        Assert.True(timeRecognizion.HasValue);

        IDateOffsetCalculation composite = CreateCompositeConverter();
        Option<DateOffsetResult> dateOffset = composite.Convert(input, time);
        Assert.True(dateOffset.HasValue);

        WhenToNotifyTimeCalculator calculator = CreateCalculator(timeRecognizion, dateOffset);
        long unixTime = calculator.CalculateWhenToFireTime();
        DateTime newDate = unixTime.FromUnixTime();
    }

    [Fact]
    public void Week_Date_Time2_Calculation_Test()
    {
        string input = "В воскресенье в 18:36 назначить генеральную уборку.";
        DateTime date = DateTime.Now;
        long unixTimeStamp = date.ToUnixTime();
        ApplicationTime time = new("Test Zone", "Test Name", unixTimeStamp, date);

        Option<TimeRecognitionResult> timeRecognizion = TimeRecognizer.TryRecognize(input);
        Assert.True(timeRecognizion.HasValue);

        IDateOffsetCalculation composite = CreateCompositeConverter();
        Option<DateOffsetResult> dateOffset = composite.Convert(input, time);
        Assert.True(dateOffset.HasValue);

        WhenToNotifyTimeCalculator calculator = CreateCalculator(timeRecognizion, dateOffset);
        long unixTime = calculator.CalculateWhenToFireTime();
        DateTime newDate = unixTime.FromUnixTime();
    }

    [Fact]
    public void Absolute_Date_Time_Calculation_Test()
    {
        string input = "23 марта в 18:36 начать генеральную уборку.";
        DateTime date = DateTime.Now;
        long unixTimeStamp = date.ToUnixTime();
        ApplicationTime time = new("Test Zone", "Test Name", unixTimeStamp, date);

        Option<TimeRecognitionResult> timeRecognizion = TimeRecognizer.TryRecognize(input);
        Assert.True(timeRecognizion.HasValue);

        IDateOffsetCalculation composite = CreateCompositeConverter();
        Option<DateOffsetResult> dateOffset = composite.Convert(input, time);
        Assert.True(dateOffset.HasValue);

        WhenToNotifyTimeCalculator calculator = CreateCalculator(timeRecognizion, dateOffset);
        long unixTime = calculator.CalculateWhenToFireTime();
        DateTime newDate = unixTime.FromUnixTime();
        Assert.Equal(23, newDate.Day);
        Assert.Equal(3, newDate.Month);
        Assert.Equal(date.Year, newDate.Year);
        Assert.Equal(18, newDate.Hour);
        Assert.Equal(36, newDate.Minute);
    }
}
