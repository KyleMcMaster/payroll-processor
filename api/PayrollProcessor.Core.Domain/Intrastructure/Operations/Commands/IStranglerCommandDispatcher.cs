using System.Threading;
using CSharpFunctionalExtensions;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
public interface IStranglerCommandDispatcher
{
    Result Dispatch(ICommand command, CancellationToken token = default);

    Result<TResponse> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default);
}
