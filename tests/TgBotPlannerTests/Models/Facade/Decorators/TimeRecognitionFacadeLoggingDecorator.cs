using System.Diagnostics;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Serilog;
using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models.Facade.Decorators;

public sealed class TimeRecognitionFacadeLoggingDecorator(
    ITimeRecognitionFacade facade,
    ILogger logger
) : ITimeRecognitionFacade
{
    private readonly ITimeRecognitionFacade _facade = facade;
    private readonly ILogger _logger = logger;

    public async Task<TimeRecognitionTicket> CreateRecognitionTicket(string message)
    {
        _logger.Information(
            "{Context} attempt to recognize sentence: {Message}",
            nameof(CreateRecognitionTicket),
            message
        );
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(message);
        Action<ILogger> action = ticket switch
        {
            UnknownRecognitionTicket => (logger) =>
                logger.Error(
                    "{Context} message has no time included.",
                    nameof(CreateRecognitionTicket)
                ),
            SingleTimeRecognitionTicket => (logger) =>
                logger.Information(
                    "{Context} recognized as single time.",
                    nameof(CreateRecognitionTicket)
                ),
            PeriodicTimeRecognitionTicket => (logger) =>
                logger.Information(
                    "{Context} recognized periodic time.",
                    nameof(CreateRecognitionTicket)
                ),
            _ => throw new UnreachableException(),
        };
        action(_logger);
        return ticket;
    }

    public async Task<RecognitionMetadataCollection> CollectMetadata(TimeRecognitionTicket ticket)
    {
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        if (collection.Count == 0)
        {
            _logger.Warning("{Context} has no recognized metadata.", nameof(CollectMetadata));
            return collection;
        }

        foreach (RecognitionMetadata metadata in collection)
            GetMetadataLoggingActionByMatch(metadata)(_logger);
        return collection;
    }

    public ApplicationTime GetApplicationTimeWithOffset(
        RecognitionMetadataCollection collection,
        ApplicationTime current
    )
    {
        ApplicationTime time = _facade.GetApplicationTimeWithOffset(collection, current);
        _logger.Information(
            "{Context} calculated time offset: {Offset}",
            nameof(GetApplicationTimeWithOffset),
            time.ToString()
        );
        return time;
    }

    public async Task<Result<ApplicationTime>> CreateRecognitionTime(
        string message,
        ApplicationTime time
    )
    {
        Result<ApplicationTime> result = await _facade.CreateRecognitionTime(message, time);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Error(
                    "{Context}. {Error}",
                    nameof(CreateRecognitionTime),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    "{Context}. Recognized: {Time}",
                    nameof(CreateRecognitionTime),
                    result.Value
                )
            )
            .BackToResult();
    }

    private static Action<ILogger> GetMetadataLoggingActionByMatch(RecognitionMetadata metadata) =>
        metadata.Recognition switch
        {
            DayOfWeekRecognition dof => (logger) =>
                logger.Information(
                    "{Context} recognized day of week: {Dof}",
                    nameof(CollectMetadata),
                    dof.DayOfWeek
                ),
            MonthRecognition month => (logger) =>
                logger.Information(
                    "{Context} recognized month number: {Month} with day: {Day}",
                    nameof(CollectMetadata),
                    month.Month,
                    month.MonthDay
                ),
            RelativeRecognition relative => (logger) =>
                logger.Information(
                    "{Context} recognized relative offset days: {Rel}",
                    nameof(CollectMetadata),
                    relative.DaysOffset
                ),
            SpecificTimeRecognition time => (logger) =>
                logger.Information(
                    "{Context} recognized time: {Hours}:{Minutes}",
                    nameof(CollectMetadata),
                    time.Hours,
                    time.Minutes
                ),
            _ => (logger) =>
                logger.Warning(
                    "{Context} unknown recognition metadata match.",
                    nameof(CollectMetadata)
                ),
        };
}
