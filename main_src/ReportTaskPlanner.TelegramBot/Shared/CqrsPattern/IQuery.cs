namespace ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;

public interface IQuery<TQueryResult> { }

public interface IQueryHandler<TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>
{
    public Task<TQueryResult> Handle(TQuery query);
}
