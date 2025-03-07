using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.UseCases.Abstractions.Handlers;

public interface IRequest;

public interface IHandler<in TRequest>
    where TRequest : class, IRequest
{
    public Task<Result> Handle(TRequest request, CancellationToken ct = default);
}

public interface IRequest<TResult>;

public interface IHandler<in TRequest, TResult>
    where TRequest : class, IRequest<TResult>
{
    public Task<Result<TResult>> Handle(TRequest request, CancellationToken ct = default);
}
