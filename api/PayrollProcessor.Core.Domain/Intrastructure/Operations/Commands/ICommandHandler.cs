using System.Threading;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    TryAsync<Unit> Execute(TCommand command, CancellationToken token);
}

public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    TryAsync<TResponse> Execute(TCommand command, CancellationToken token);
}
