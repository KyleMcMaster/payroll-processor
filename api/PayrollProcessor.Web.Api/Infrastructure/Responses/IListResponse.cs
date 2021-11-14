using System.Collections.Generic;

namespace PayrollProcessor.Web.Api.Infrastructure.Responses;

public interface IListResponse<TItem>
{
    IEnumerable<TItem> Data { get; }
}
