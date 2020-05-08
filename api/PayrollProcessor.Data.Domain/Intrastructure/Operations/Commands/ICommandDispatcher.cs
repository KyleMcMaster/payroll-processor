using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands
{
    public interface ICommandDispatcher
    {
        Task<Either<TError, Unit>> Dispatch<TError>(ICommand<TError> command, CancellationToken token = default);

        Task<Either<TError, TResponse>> Dispatch<TError, TResponse>(ICommand<TError, TResponse> command, CancellationToken token = default);
    }
}
