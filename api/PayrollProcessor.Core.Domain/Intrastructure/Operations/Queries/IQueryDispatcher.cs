using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

public interface IQueryDispatcher
{
    Task<Maybe<TResponse>> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default);
}
