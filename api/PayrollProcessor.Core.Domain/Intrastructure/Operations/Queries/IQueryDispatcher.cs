using System.Threading;
using CSharpFunctionalExtensions;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

public interface IQueryDispatcher
{
    TryOptionAsync<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default);
    // TODO: Rename to Dispatch when language ext has been refactored out
    Result<Maybe<TResponse>> DispatchQuery<TResponse>(IQuery<TResponse> query, CancellationToken token = default);
}
