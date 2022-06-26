using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly ServiceProviderDelegate serviceProvider;
    private static readonly ConcurrentDictionary<Type, object> commandHandlers = new ConcurrentDictionary<Type, object>();

    public CommandDispatcher(ServiceProviderDelegate serviceProvider)
    {
        Guard.Against.Null(serviceProvider, nameof(serviceProvider));

        this.serviceProvider = serviceProvider;
    }

    public Task<Result> Dispatch(ICommand command, CancellationToken token = default)
    {
        Guard.Against.Null(command, nameof(command));

        var commandType = command.GetType();

        var handler = (CommandHandlerWrapper)commandHandlers
            .GetOrAdd(
                commandType,
#pragma warning disable CS8603 // Possible null reference return.
                    t => Activator
                        .CreateInstance(typeof(CommandHandlerWrapperImpl<>)
                        .MakeGenericType(commandType)));
#pragma warning restore CS8603 // Possible null reference return.

        return handler.Dispatch(command, serviceProvider, token);
    }

    public Task<Result<TResponse>> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default)
    {
        Guard.Against.Null(command, nameof(command));

        var commandType = command.GetType();

        var handler = (CommandHandlerWrapper<TResponse>)commandHandlers
            .GetOrAdd(
                commandType,
#pragma warning disable CS8603 // Possible null reference return.
                    t => Activator
                        .CreateInstance(typeof(CreateCommandHandlerWrapperImpl<,>)
                        .MakeGenericType(commandType, typeof(TResponse))));
#pragma warning restore CS8603 // Possible null reference return.

        return handler.Dispatch(command, serviceProvider, token);
    }
}

internal abstract class CommandHandlerWrapper : HandlerBase
{
    public abstract Task<Result> Dispatch(ICommand command, ServiceProviderDelegate serviceProvider, CancellationToken token);
}

internal class CommandHandlerWrapperImpl<TCommand> : CommandHandlerWrapper
    where TCommand : ICommand
{
    public override Task<Result> Dispatch(ICommand command, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
        GetHandler<ICommandHandler<TCommand>>(serviceProvider).Execute((TCommand)command, token);
}

internal abstract class CommandHandlerWrapper<TResponse> : HandlerBase
{
    public abstract Task<Result<TResponse>> Dispatch(ICommand<TResponse> command, ServiceProviderDelegate serviceProvider, CancellationToken token);
}

internal class CreateCommandHandlerWrapperImpl<TCommand, TResponse> : CommandHandlerWrapper<TResponse>
    where TCommand : ICommand<TResponse>
{
    public override Task<Result<TResponse>> Dispatch(ICommand<TResponse> command, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
        GetHandler<ICommandHandler<TCommand, TResponse>>(serviceProvider).Execute((TCommand)command, token);
}
