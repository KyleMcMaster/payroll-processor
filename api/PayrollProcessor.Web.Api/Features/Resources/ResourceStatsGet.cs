using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Resources;

public class ResourceStatsGet : BaseAsyncEndpoint
    .WithoutRequest
    .WithResponse<ResourceStatsResponse>
{
    private readonly IQueryDispatcher dispatcher;

    public ResourceStatsGet(IQueryDispatcher dispatcher)
    {
        Guard.Against.Null(dispatcher, nameof(dispatcher));

        this.dispatcher = dispatcher;
    }

    [HttpGet("resources/stats"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Gets resource stats",
        Description = "Gets current data persistence resource stats (row counts)",
        OperationId = "Resource.Stats.Get",
        Tags = new[] { "Resources" })
    ]
    public override Task<ActionResult<ResourceStatsResponse>> HandleAsync(CancellationToken token) =>
        dispatcher
            .Dispatch(new ResourceCountQuery(), token)
            .Map(resp => new ResourceStatsResponse(resp.TotalEmployees, resp.TotalPayrolls))
            .Match<ResourceStatsResponse, ActionResult<ResourceStatsResponse>>(
                e => e,
                () => NotFound($"Resource stats"),
                ex => new APIErrorResult(ex.Message));
}

public class ResourceStatsResponse
{
    public ResourceStatsResponse(int totalEmployees, int totalPayrolls)
    {
        TotalEmployees = totalEmployees;
        TotalPayrolls = totalPayrolls;
    }

    public int TotalEmployees { get; }
    public int TotalPayrolls { get; }
}
