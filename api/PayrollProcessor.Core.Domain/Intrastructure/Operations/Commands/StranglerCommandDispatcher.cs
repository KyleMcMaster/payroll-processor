using System;
using System.Collections.Concurrent;
using System.Threading;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

/// <summary>
/// TODO: Temporarily named after the Strangler Fig Pattern as this serves as an implementation of <see cref="ICommandDispatcher"/> migrating to CSharpFunctionalExtensions from LanguageExt.
/// </summary>
public class StranglerCommandDispatcher : IStranglerCommandDispatcher
{
    private readonly ServiceProviderDelegate serviceProvider;
    private static readonly ConcurrentDictionary<Type, object> commandHandlers = new ConcurrentDictionary<Type, object>();

    public StranglerCommandDispatcher(ServiceProviderDelegate serviceProvider)
    {
        Guard.Against.Null(serviceProvider, nameof(serviceProvider));

        this.serviceProvider = serviceProvider;
    }

    public Result Dispatch(ICommand command, CancellationToken token = default)
    {
        Guard.Against.Null(command, nameof(command));

        var commandType = command.GetType();

        var handler = (StranglerCommandHandlerWrapper)commandHandlers
            .GetOrAdd(
                commandType,
#pragma warning disable CS8603 // Possible null reference return.
                    t => Activator
                        .CreateInstance(typeof(StranglerCommandHandlerWrapperImpl<>)
                        .MakeGenericType(commandType)));
#pragma warning restore CS8603 // Possible null reference return.

        return handler.Dispatch(command, serviceProvider, token);
    }

    public Result<TResponse> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default)
    {
        Guard.Against.Null(command, nameof(command));

        var commandType = command.GetType();

        var handler = (StranglerCommandHandlerWrapper<TResponse>)commandHandlers
            .GetOrAdd(
                commandType,
#pragma warning disable CS8603 // Possible null reference return.
                    t => Activator
                        .CreateInstance(typeof(StranglerCreateCommandHandlerWrapperImpl<,>)
                        .MakeGenericType(commandType, typeof(TResponse))));
#pragma warning restore CS8603 // Possible null reference return.

        return handler.Dispatch(command, serviceProvider, token);
    }
}

internal abstract class StranglerCommandHandlerWrapper : HandlerBase
{
    public abstract Result Dispatch(ICommand command, ServiceProviderDelegate serviceProvider, CancellationToken token);
}

internal class StranglerCommandHandlerWrapperImpl<TCommand> : StranglerCommandHandlerWrapper
    where TCommand : ICommand
{
    public override Result Dispatch(ICommand command, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
        GetHandler<IStranglerCommandHandler<TCommand>>(serviceProvider).Execute((TCommand)command, token);
}

internal abstract class StranglerCommandHandlerWrapper<TResponse> : HandlerBase
{
    public abstract Result<TResponse> Dispatch(ICommand<TResponse> command, ServiceProviderDelegate serviceProvider, CancellationToken token);
}

internal class StranglerCreateCommandHandlerWrapperImpl<TCommand, TResponse> : StranglerCommandHandlerWrapper<TResponse>
    where TCommand : ICommand<TResponse>
{
    public override Result<TResponse> Dispatch(ICommand<TResponse> command, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
        GetHandler<IStranglerCommandHandler<TCommand, TResponse>>(serviceProvider).Execute((TCommand)command, token);
}
