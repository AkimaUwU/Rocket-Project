using System;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public interface IQueriesHandler <TQueries,TResult> where TQueries:IQueries <TResult>
{
    Task <Result<TResult>> Handle (TQueries command);
}
