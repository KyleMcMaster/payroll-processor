using System;
using System.Collections.Concurrent;
using System.Threading;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using LanguageExt;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly ServiceProviderDelegate serviceProvider;
    private static readonly ConcurrentDictionary<Type, object> queryHandlers = new ConcurrentDictionary<Type, object>();

    public QueryDispatcher(ServiceProviderDelegate serviceProvider)
    {
        Guard.Against.Null(serviceProvider, nameof(serviceProvider));

        this.serviceProvider = serviceProvider;
    }

    public TryOptionAsync<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default)
    {
        Guard.Against.Null(query, nameof(query));

        var queryType = query.GetType();

        var handler = (QueryHandlerWrapper<TResponse>)queryHandlers
            .GetOrAdd(
                queryType,
#pragma warning disable CS8603 // Possible null reference return.
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerWrapperImpl<,>)
                        .MakeGenericType(queryType, typeof(TResponse))));
#pragma warning restore CS8603 // Possible null reference return.

        return handler.Dispatch(query, serviceProvider, token);
    }

    public Result<Maybe<TResponse>> DispatchQuery<TResponse>(IQuery<TResponse> query, CancellationToken token = default)
    {
        Guard.Against.Null(query, nameof(query));

        var queryType = query.GetType();

        var handler = (QueryHandlerWrapper<TResponse>)queryHandlers
            .GetOrAdd(
                queryType,
#pragma warning disable CS8603 // Possible null reference return.
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerWrapperImpl<,>)
                        .MakeGenericType(queryType, typeof(TResponse))));
#pragma warning restore CS8603 // Possible null reference return.

        return handler.DispatchQuery(query, serviceProvider, token);
    }
}

internal abstract class QueryHandlerWrapper<TResponse> : HandlerBase
{
    public abstract TryOptionAsync<TResponse> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceProvider, CancellationToken token);
    public abstract Result<Maybe<TResponse>> DispatchQuery(IQuery<TResponse> query, ServiceProviderDelegate serviceProvider, CancellationToken token);
}

internal class QueryHandlerWrapperImpl<TQuery, TResponse> : QueryHandlerWrapper<TResponse>
    where TQuery : IQuery<TResponse>
{
    public override TryOptionAsync<TResponse> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
        GetHandler<IQueryHandler<TQuery, TResponse>>(serviceProvider).Execute((TQuery)query, token);

    public override Result<Maybe<TResponse>> DispatchQuery(IQuery<TResponse> query, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
        GetHandler<IQueryHandler<TQuery, TResponse>>(serviceProvider).ExecuteQuery((TQuery)query, token);
}
