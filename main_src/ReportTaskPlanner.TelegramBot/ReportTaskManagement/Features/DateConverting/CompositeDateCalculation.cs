using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting;

public class CompositeDateConverter : IDateOffsetCalculation
{
    private readonly List<IDateOffsetCalculation> _calculations;

    private CompositeDateConverter(List<IDateOffsetCalculation> converters) =>
        _calculations = converters;

    public CompositeDateConverter() => _calculations = [];

    public CompositeDateConverter With(IDateOffsetCalculation converter)
    {
        _calculations.Add(converter);
        return new CompositeDateConverter(_calculations);
    }

    public Option<DateOffsetResult> Convert(string stringDate, ApplicationTime time)
    {
        foreach (var converter in _calculations)
        {
            Option<DateOffsetResult> result = converter.Convert(stringDate, time);
            if (result.HasValue)
                return result;
        }

        return Option<DateOffsetResult>.None();
    }
}
