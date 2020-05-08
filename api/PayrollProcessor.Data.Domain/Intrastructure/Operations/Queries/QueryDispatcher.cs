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

        public Task<Either<TError, TResponse>> Dispatch<TError, TResponse>(IQuery<TError, TResponse> query)
        {
            Guard.Against.Null(query, nameof(query));

            var queryType = query.GetType();

            var handler = (QueryHandlerWrapper<TError, TResponse>)queryHandlers
                .GetOrAdd(
                    queryType,
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerWrapperImpl<,,>)
                        .MakeGenericType(queryType, typeof(TError), typeof(TResponse))));

            return handler.Dispatch(query, serviceFactory);
        }
    }

    internal abstract class QueryHandlerWrapper<TError, TResponse> : HandlerBase
    {
        public abstract Task<Either<TError, TResponse>> Dispatch(IQuery<TError, TResponse> query, ServiceProviderDelegate serviceFactory);
    }

    internal class QueryHandlerWrapperImpl<TQuery, TError, TResponse> : QueryHandlerWrapper<TError, TResponse>
        where TQuery : IQuery<TError, TResponse>
    {
        public override Task<Either<TError, TResponse>> Dispatch(IQuery<TError, TResponse> query, ServiceProviderDelegate serviceFactory) =>
            GetHandler<IQueryHandler<TQuery, TError, TResponse>>(serviceFactory).Execute((TQuery)query);
    }
}
