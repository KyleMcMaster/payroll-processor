using System.Threading;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        TryOptionAsync<Unit> Execute(TCommand command, CancellationToken token);
    }

    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        TryOptionAsync<TResponse> Execute(TCommand command, CancellationToken token);
    }
}
