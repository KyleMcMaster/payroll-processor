using System.Collections.Generic;

namespace PayrollProcessor.Api.Infrastructure.Responses
{
    public interface IListResponse<TItem>
    {
        IEnumerable<TItem> Data { get; }
    }
}
