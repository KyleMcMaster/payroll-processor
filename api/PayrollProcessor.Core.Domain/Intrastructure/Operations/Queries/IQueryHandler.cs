using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    TryOptionAsync<TResponse> Execute(TQuery query, CancellationToken token);
    Task<Maybe<TResponse>> ExecuteQuery(TQuery query, CancellationToken token);
}
