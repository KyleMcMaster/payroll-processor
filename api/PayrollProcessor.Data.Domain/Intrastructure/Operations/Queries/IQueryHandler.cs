using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryHandler<in TQuery, TError, TResponse> where TQuery : IQuery<TError, TResponse>
    {
        Task<Either<TError, TResponse>> Execute(TQuery query, CancellationToken token = default);
    }
}
