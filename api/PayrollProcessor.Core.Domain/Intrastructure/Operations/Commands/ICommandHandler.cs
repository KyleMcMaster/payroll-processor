using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<Result> Execute(TCommand command, CancellationToken token);
}

public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Execute(TCommand command, CancellationToken token);
}
