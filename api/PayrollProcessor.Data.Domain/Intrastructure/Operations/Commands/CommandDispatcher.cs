using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ServiceProviderDelegate serviceFactory;
        private static readonly ConcurrentDictionary<Type, object> commandHandlers = new ConcurrentDictionary<Type, object>();

        public CommandDispatcher(ServiceProviderDelegate serviceFactory) =>
            this.serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));

        public Task<Either<TError, Unit>> Dispatch<TError>(ICommand<TError> command, CancellationToken token = default)
        {
            Guard.Against.Null(command, nameof(command));

            var commandType = command.GetType();

            var handler = (CommandHandlerWrapper<TError>)commandHandlers
                .GetOrAdd(
                    commandType,
                    t => Activator
                        .CreateInstance(typeof(CommandHandlerWrapperImpl<,>)
                        .MakeGenericType(commandType, typeof(TError))));

            return handler.Dispatch(command, serviceFactory, token);
        }

        public Task<Either<TError, TResponse>> Dispatch<TError, TResponse>(ICommand<TError, TResponse> command, CancellationToken token = default)
        {
            Guard.Against.Null(command, nameof(command));

            var commandType = command.GetType();

            var handler = (CommandHandlerWrapper<TError, TResponse>)commandHandlers
                .GetOrAdd(
                    commandType,
                    t => Activator
                        .CreateInstance(typeof(CreateCommandHandlerWrapperImpl<,,>)
                        .MakeGenericType(commandType, typeof(TError), typeof(TResponse))));

            return handler.Dispatch(command, serviceFactory, token);
        }
    }

    internal abstract class CommandHandlerWrapper<TError> : HandlerBase
    {
        public abstract Task<Either<TError, Unit>> Dispatch(ICommand<TError> command, ServiceProviderDelegate serviceFactory, CancellationToken token);
    }

    internal class CommandHandlerWrapperImpl<TCommand, TError> : CommandHandlerWrapper<TError>
        where TCommand : ICommand<TError>
    {
        public override Task<Either<TError, Unit>> Dispatch(ICommand<TError> command, ServiceProviderDelegate serviceFactory, CancellationToken token) =>
            GetHandler<ICommandHandler<TCommand, TError>>(serviceFactory).Execute((TCommand)command, token);
    }

    internal abstract class CommandHandlerWrapper<TError, TResponse> : HandlerBase
    {
        public abstract Task<Either<TError, TResponse>> Dispatch(ICommand<TError, TResponse> command, ServiceProviderDelegate serviceFactory, CancellationToken token);
    }

    internal class CreateCommandHandlerWrapperImpl<TCommand, TError, TResponse> : CommandHandlerWrapper<TError, TResponse>
        where TCommand : ICommand<TError, TResponse>
    {
        public override Task<Either<TError, TResponse>> Dispatch(ICommand<TError, TResponse> command, ServiceProviderDelegate serviceFactory, CancellationToken token) =>
            GetHandler<ICommandHandler<TCommand, TError, TResponse>>(serviceFactory).Execute((TCommand)command, token);
    }

    internal abstract class CommandHandlerSyncWrapper<TError> : HandlerBase
    {
        public abstract Either<TError, Unit> Dispatch(ICommandSync<TError> command, ServiceProviderDelegate serviceFactory, CancellationToken token);
    }
}
