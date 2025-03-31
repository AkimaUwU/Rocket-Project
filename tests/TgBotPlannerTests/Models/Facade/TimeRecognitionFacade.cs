using System.Diagnostics;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using TgBotPlannerTests.Models.Recognitions;
using TgBotPlannerTests.Models.RecognitionStrategies;
using TgBotPlannerTests.Models.Recognizers;

namespace TgBotPlannerTests.Models.Facade;

public sealed class TimeRecognitionFacade : ITimeRecognitionFacade
{
    private readonly RecognitionProcessor _processor;
    private readonly Dictionary<Type, ITimeRecognizer> _recognizers = [];
    private readonly Dictionary<Type, IRecognitionStrategy> _strategies = [];

    public TimeRecognitionFacade()
    {
        _processor = new RecognitionProcessor();
        _recognizers.Add(typeof(DayOfWeekRecognizer), new DayOfWeekRecognizer());
        _recognizers.Add(typeof(MonthRecognizer), new MonthRecognizer());
        _recognizers.Add(typeof(PeriodicTimeRecognizer), new PeriodicTimeRecognizer());
        _recognizers.Add(typeof(RelativeDateRecognizer), new RelativeDateRecognizer());
        _recognizers.Add(typeof(TimeRecognizer), new TimeRecognizer());
        _strategies.Add(typeof(RawStringRecognitionStrategy), new RawStringRecognitionStrategy());
        _strategies.Add(typeof(ChunkRecognitionStrategy), new ChunkRecognitionStrategy());
    }

    public async Task<Result<ApplicationTime>> CreateRecognitionTime(
        string message,
        ApplicationTime time
    )
    {
        TimeRecognitionTicket ticket = await CreateRecognitionTicket(message);
        if (ticket is UnknownRecognitionTicket)
            return new Error("Время не было распознано.");
        RecognitionMetadataCollection collection = await CollectMetadata(ticket);
        if (collection.Count == 0)
            return new Error("Время не было распознано.");
        ApplicationTime calculated = GetApplicationTimeWithOffset(collection, time);
        return calculated;
    }

    public async Task<TimeRecognitionTicket> CreateRecognitionTicket(string message)
    {
        bool hasTime = (await HasTime(message)).HasValue;
        bool isPeriodic = (await IsPeriodic(message)).HasValue;

        return hasTime switch
        {
            false when !isPeriodic => new UnknownRecognitionTicket(),
            false when isPeriodic => new PeriodicTimeRecognitionTicket(message),
            true when !isPeriodic => new SingleTimeRecognitionTicket(message),
            true when isPeriodic => new PeriodicTimeRecognitionTicket(message),
            _ => new UnknownRecognitionTicket(),
        };
    }

    public async Task<RecognitionMetadataCollection> CollectMetadata(
        TimeRecognitionTicket ticket
    ) =>
        ticket switch
        {
            SingleTimeRecognitionTicket s => await FromSingleTicket(s),
            PeriodicTimeRecognitionTicket p => await FromPeriodicTicket(p),
            UnknownRecognitionTicket => FromUnknownTicket(),
            _ => throw new UnreachableException(),
        };

    public ApplicationTime GetApplicationTimeWithOffset(
        RecognitionMetadataCollection collection,
        ApplicationTime current
    ) => collection.Aggregate(current, (node, item) => item.Recognition.Calculate(node));

    private async Task<Option<TimeRecognition>> IsRelative(string message)
    {
        TimeRecognition recognition = await PerformRecognition(
            message,
            _strategies[typeof(ChunkRecognitionStrategy)],
            _recognizers[typeof(RelativeDateRecognizer)]
        );
        return FromValidation(recognition);
    }

    private async Task<Option<TimeRecognition>> HasMonth(string message)
    {
        TimeRecognition recognition = await PerformRecognition(
            message,
            _strategies[typeof(RawStringRecognitionStrategy)],
            _recognizers[typeof(MonthRecognizer)]
        );
        Option<TimeRecognition> recognitionResult = FromValidation(recognition);
        return recognitionResult.HasValue
            ? recognitionResult
            : FromValidation(
                await PerformRecognition(
                    message,
                    _strategies[typeof(ChunkRecognitionStrategy)],
                    _recognizers[typeof(MonthRecognizer)]
                )
            );
    }

    private async Task<Option<TimeRecognition>> HasDayOfWeek(string message)
    {
        TimeRecognition recognition = await PerformRecognition(
            message,
            _strategies[typeof(ChunkRecognitionStrategy)],
            _recognizers[typeof(DayOfWeekRecognizer)]
        );
        return FromValidation(recognition);
    }

    private async Task<Option<TimeRecognition>> HasTime(string input)
    {
        TimeRecognition recognition = await PerformRecognition(
            input,
            _strategies[typeof(RawStringRecognitionStrategy)],
            _recognizers[typeof(TimeRecognizer)]
        );
        return FromValidation(recognition);
    }

    private async Task<Option<TimeRecognition>> IsPeriodic(string input)
    {
        TimeRecognition recognition = await PerformRecognition(
            input,
            _strategies[typeof(RawStringRecognitionStrategy)],
            _recognizers[typeof(PeriodicTimeRecognizer)]
        );
        return FromValidation(recognition);
    }

    private async Task<TimeRecognition> PerformRecognition(
        string input,
        IRecognitionStrategy strategy,
        params ITimeRecognizer[] recognizers
    )
    {
        _processor.Strategy = strategy;
        _processor.AddRecognizer(recognizers);
        TimeRecognition recognition = await _processor.PerformRecognition(input);
        _processor.ResetRecognizers();
        return recognition;
    }

    private async Task<RecognitionMetadataCollection> FromSingleTicket(
        SingleTimeRecognitionTicket ticket
    ) => await FromMessage(ticket.Message);

    private async Task<RecognitionMetadataCollection> FromPeriodicTicket(
        PeriodicTimeRecognitionTicket ticket
    )
    {
        RecognitionMetadataCollection collection = await FromMessage(ticket.Message);
        if (collection.Count != 0)
            return collection;
        string message = ticket.Message;
        Option<TimeRecognition> periodic = FromValidation(
            await PerformRecognition(
                message,
                _strategies[typeof(RawStringRecognitionStrategy)],
                _recognizers[typeof(PeriodicTimeRecognizer)]
            )
        );
        return !periodic.HasValue
            ? collection
            : new RecognitionMetadataCollection([new RecognitionMetadata(periodic.Value)]);
    }

    private async Task<RecognitionMetadataCollection> FromMessage(string message)
    {
        List<RecognitionMetadata> metadata = new(4);
        Option<TimeRecognition> isRelative = await IsRelative(message);
        Option<TimeRecognition> hasMonth = await HasMonth(message);
        Option<TimeRecognition> hasDayOfWeek = await HasDayOfWeek(message);
        Option<TimeRecognition> hasTime = await HasTime(message);
        if (!hasTime.HasValue && !isRelative.HasValue)
            return new RecognitionMetadataCollection([]);
        if (isRelative.HasValue)
            metadata.Add(new RecognitionMetadata(isRelative.Value));
        if (hasMonth.HasValue)
            metadata.Add(new RecognitionMetadata(hasMonth.Value));
        if (hasDayOfWeek.HasValue)
            metadata.Add(new RecognitionMetadata(hasDayOfWeek.Value));
        metadata.Add(new RecognitionMetadata(hasTime.Value));
        return new RecognitionMetadataCollection(metadata);
    }

    private static RecognitionMetadataCollection FromUnknownTicket() => new([]);

    private static Option<TimeRecognition> FromValidation(TimeRecognition recognition) =>
        RecognitionValidator.IsRecognized(recognition)
            ? Option<TimeRecognition>.Some(recognition)
            : Option<TimeRecognition>.None();
}
