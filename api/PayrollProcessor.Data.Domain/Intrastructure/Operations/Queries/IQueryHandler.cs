using System.Threading;
using LanguageExt;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        TryOptionAsync<TResponse> Execute(TQuery query, CancellationToken token = default);
    }
}
