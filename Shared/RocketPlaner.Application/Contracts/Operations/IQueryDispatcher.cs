using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public interface IQueryDispatcher
{
    Task<Result<TQueryResult>> Resolve<TQuery, TQueryResult>(TQuery command)
        where TQuery : IQuery<TQueryResult>;
}
