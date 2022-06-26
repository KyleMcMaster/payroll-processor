using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

public interface ICommandDispatcher
{
    Task<Result> Dispatch(ICommand command, CancellationToken token = default);

    Task<Result<TResponse>> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default);
}
