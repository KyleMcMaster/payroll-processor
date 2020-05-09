using System;
using System.Collections.Concurrent;
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly ServiceProviderDelegate serviceFactory;
        private static readonly ConcurrentDictionary<Type, object> queryHandlers = new ConcurrentDictionary<Type, object>();

        public QueryDispatcher(ServiceProviderDelegate serviceFactory)
        {
            Guard.Against.Null(serviceFactory, nameof(serviceFactory));

            this.serviceFactory = serviceFactory;
        }

        public TryOptionAsync<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default)
        {
            Guard.Against.Null(query, nameof(query));

            var queryType = query.GetType();

            var handler = (QueryHandlerWrapper<TResponse>)queryHandlers
                .GetOrAdd(
                    queryType,
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerWrapperImpl<,>)
                        .MakeGenericType(queryType, typeof(TResponse))));

            return handler.Dispatch(query, serviceFactory, token);
        }
    }

    internal abstract class QueryHandlerWrapper<TResponse> : HandlerBase
    {
        public abstract TryOptionAsync<TResponse> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceFactory, CancellationToken token);
    }

    internal class QueryHandlerWrapperImpl<TQuery, TResponse> : QueryHandlerWrapper<TResponse>
        where TQuery : IQuery<TResponse>
    {
        public override TryOptionAsync<TResponse> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceFactory, CancellationToken token) =>
            GetHandler<IQueryHandler<TQuery, TResponse>>(serviceFactory).Execute((TQuery)query, token);
    }
}
