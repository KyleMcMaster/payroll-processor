using System.Threading;
using CSharpFunctionalExtensions;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
public interface IStranglerCommandHandler<TCommand> where TCommand : ICommand
{
    Result Execute(TCommand command, CancellationToken token);
}

public interface IStranglerCommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Result<TResponse> Execute(TCommand command, CancellationToken token);
}
