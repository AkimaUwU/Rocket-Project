using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime.Decorators;

public sealed class UpdateApplicationTimeTimeZoneDbRequesting
    : ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>
{
    private readonly ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> _handler;
    private readonly UpdateApplicationTimeSharedContext _context;

    public UpdateApplicationTimeTimeZoneDbRequesting(
        ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> handler,
        UpdateApplicationTimeSharedContext context
    )
    {
        _handler = handler;
        _context = context;
    }

    public async Task<Result<ApplicationTime>> Handle(UpdateApplicationTimeCommand command)
    {
        ApplicationTime time = command.Current;
        TimeZoneDbOptions options = command.Options;
        using HttpClient client = new HttpClient();
        string url = CreateUrlString(time, options);
        HttpResponseMessage message = await client.GetAsync(url);
        await Task.Delay(TimeSpan.FromSeconds(3)); // free api limitation.
        if (!message.IsSuccessStatusCode)
        {
            string json = await message.Content.ReadAsStringAsync();
            Error error = new Error($"Ответ от Time Zone Db с ошибкой. Ответ: {json}");
            _context.SetError(error);
        }

        _context.TimeZoneDbJson = Option<string>.Some(await message.Content.ReadAsStringAsync());
        return await _handler.ExecuteNextWithErrorChecking(command, _context);
    }

    private string CreateUrlString(ApplicationTime time, TimeZoneDbOptions options) =>
        $"https://api.timezonedb.com/v2.1/list-time-zone&key={options.Token}&format=json&country=RU&zone={time.ZoneName}";
}
