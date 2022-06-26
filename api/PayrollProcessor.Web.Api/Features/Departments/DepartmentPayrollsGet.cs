using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Web.Api.Infrastructure.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Departments;

public class DepartmentPayrollsGet : EndpointBaseAsync
    .WithRequest<DepartmentPayrollsRequest>
    .WithActionResult<DepartmentPayrollsResponse>
{
    private readonly IQueryDispatcher dispatcher;

    public DepartmentPayrollsGet(IQueryDispatcher dispatcher)
    {
        Guard.Against.Null(dispatcher, nameof(dispatcher));

        this.dispatcher = dispatcher;
    }

    [HttpGet("departments/payrolls"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Gets department payrolls",
        Description = "Gets all payrolls matching request parameters in the given department",
        OperationId = "DepartmentPayrolls.GetAll",
        Tags = new[] { "Payrolls", "Departments" })
    ]
    public override async Task<ActionResult<DepartmentPayrollsResponse>> HandleAsync([FromQuery] DepartmentPayrollsRequest request, CancellationToken token) =>
       (await Result.Try(() => dispatcher.Dispatch(new DepartmentPayrollsQuery(request.Count, request.Department, request.CheckDateFrom, request.CheckDateTo), token)))
            .Match<IEnumerable<DepartmentPayroll>, ActionResult<DepartmentPayrollsResponse>>(
                e => new DepartmentPayrollsResponse(e.Value),
                () => NotFound(),
                ex => new APIErrorResult(ex));
}

public class DepartmentPayrollsRequest
{
    public int Count { get; set; }
    public string Department { get; set; } = "";
    public DateTime? CheckDateFrom { get; set; }
    public DateTime? CheckDateTo { get; set; }
}

public class DepartmentPayrollsResponse : IListResponse<DepartmentPayroll>
{
    public IEnumerable<DepartmentPayroll> Data { get; }

    public DepartmentPayrollsResponse(IEnumerable<DepartmentPayroll> data)
    {
        Guard.Against.Null(data, nameof(data));

        Data = data;
    }
}
