using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries
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

        public TryOptionAsync<TResponse> Dispatch<TResponse>(IQuery<TResponse> query)
        {
            Guard.Against.Null(query, nameof(query));

            var queryType = query.GetType();

            var handler = (QueryHandlerWrapper<TResponse>)queryHandlers
                .GetOrAdd(
                    queryType,
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerWrapperImpl<,>)
                        .MakeGenericType(queryType, typeof(TResponse))));

            return handler.Dispatch(query, serviceFactory);
        }
    }

    internal abstract class QueryHandlerWrapper<TResponse> : HandlerBase
    {
        public abstract TryOptionAsync<TResponse> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceFactory);
    }

    internal class QueryHandlerWrapperImpl<TQuery, TResponse> : QueryHandlerWrapper<TResponse>
        where TQuery : IQuery<TResponse>
    {
        public override TryOptionAsync<TResponse> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceFactory) =>
            GetHandler<IQueryHandler<TQuery, TResponse>>(serviceFactory).Execute((TQuery)query);
    }
}
