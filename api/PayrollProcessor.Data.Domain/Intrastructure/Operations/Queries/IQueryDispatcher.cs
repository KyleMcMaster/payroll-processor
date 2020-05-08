using LanguageExt;

namespace PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryDispatcher
    {
        TryOptionAsync<TResponse> Dispatch<TResponse>(IQuery<TResponse> query);
    }
}
