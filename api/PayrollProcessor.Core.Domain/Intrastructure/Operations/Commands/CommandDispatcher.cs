using System;
using System.Collections.Concurrent;
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ServiceProviderDelegate serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> commandHandlers = new ConcurrentDictionary<Type, object>();

        public CommandDispatcher(ServiceProviderDelegate serviceProvider)
        {
            Guard.Against.Null(serviceProvider, nameof(serviceProvider));

            this.serviceProvider = serviceProvider;
        }

        public TryAsync<Unit> Dispatch(ICommand command, CancellationToken token = default)
        {
            Guard.Against.Null(command, nameof(command));

            var commandType = command.GetType();

            var handler = (CommandHandlerWrapper)commandHandlers
                .GetOrAdd(
                    commandType,
                    t => Activator
                        .CreateInstance(typeof(CommandHandlerWrapperImpl<>)
                        .MakeGenericType(commandType)));

            return handler.Dispatch(command, serviceProvider, token);
        }

        public TryAsync<TResponse> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default)
        {
            Guard.Against.Null(command, nameof(command));

            var commandType = command.GetType();

            var handler = (CommandHandlerWrapper<TResponse>)commandHandlers
                .GetOrAdd(
                    commandType,
                    t => Activator
                        .CreateInstance(typeof(CreateCommandHandlerWrapperImpl<,>)
                        .MakeGenericType(commandType, typeof(TResponse))));

            return handler.Dispatch(command, serviceProvider, token);
        }
    }

    internal abstract class CommandHandlerWrapper : HandlerBase
    {
        public abstract TryAsync<Unit> Dispatch(ICommand command, ServiceProviderDelegate serviceProvider, CancellationToken token);
    }

    internal class CommandHandlerWrapperImpl<TCommand> : CommandHandlerWrapper
        where TCommand : ICommand
    {
        public override TryAsync<Unit> Dispatch(ICommand command, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
            GetHandler<ICommandHandler<TCommand>>(serviceProvider).Execute((TCommand)command, token);
    }

    internal abstract class CommandHandlerWrapper<TResponse> : HandlerBase
    {
        public abstract TryAsync<TResponse> Dispatch(ICommand<TResponse> command, ServiceProviderDelegate serviceProvider, CancellationToken token);
    }

    internal class CreateCommandHandlerWrapperImpl<TCommand, TResponse> : CommandHandlerWrapper<TResponse>
        where TCommand : ICommand<TResponse>
    {
        public override TryAsync<TResponse> Dispatch(ICommand<TResponse> command, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
            GetHandler<ICommandHandler<TCommand, TResponse>>(serviceProvider).Execute((TCommand)command, token);
    }
}
