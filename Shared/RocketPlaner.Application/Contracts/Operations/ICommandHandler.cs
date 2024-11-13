using System;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public interface ICommandHandler<TCommand,TResult> where TCommand:ICommand <TResult>
{
    Task <Result<TResult>> Handle (TCommand command);
}
