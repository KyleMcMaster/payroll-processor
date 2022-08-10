using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<Maybe<TResponse>> ExecuteQuery(TQuery query, CancellationToken token);
}
