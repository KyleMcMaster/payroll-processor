using System.Threading;
using LanguageExt;

namespace PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands
{
    public interface ICommandDispatcher
    {
        TryOptionAsync<Unit> Dispatch(ICommand command, CancellationToken token = default);

        TryOptionAsync<TResponse> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken token = default);
    }
}
