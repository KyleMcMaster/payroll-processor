using System.Threading.Tasks;
using LanguageExt;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryDispatcher
    {
        Task<Either<TError, TResponse>> Dispatch<TError, TResponse>(IQuery<TError, TResponse> query);
    }
}
