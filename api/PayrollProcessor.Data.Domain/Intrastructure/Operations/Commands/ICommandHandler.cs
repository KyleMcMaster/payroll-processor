using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands
{
    public interface ICommandHandler<TCommand, TError> where TCommand : ICommand<TError>
    {
        Task<Either<TError, Unit>> Execute(TCommand command, CancellationToken token);
    }

    public interface ICommandHandler<TCommand, TError, TResponse> where TCommand : ICommand<TError, TResponse>
    {
        Task<Either<TError, TResponse>> Execute(TCommand command, CancellationToken token);
    }

    public interface ICommandHandlerSync<TCommand, TError> where TCommand : ICommandSync<TError>
    {
        Either<TError, Unit> Execute(TCommand command, CancellationToken token);
    }

    public interface ICommandHandlerSync<TCommand, TError, TResponse> where TCommand : ICommandSync<TError, TResponse>
    {
        Either<TError, TResponse> Execute(TCommand command, CancellationToken token);
    }
}
