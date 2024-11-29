using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public interface IQueryValidator<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public Task<bool> IsQueryValidAsync(TQuery query);

    public Error GetLastError();
}
