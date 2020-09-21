using System.Threading;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands
{
    public interface ICommandDispatcher
    {
        TryAsync<Unit> Dispatch(ICommand command, CancellationToken token = default);

        TryAsync<TResponse> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default);
    }
}
