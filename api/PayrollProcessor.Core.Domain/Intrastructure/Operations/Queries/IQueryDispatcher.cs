using System.Threading;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryDispatcher
    {
        TryOptionAsync<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default);
    }
}
