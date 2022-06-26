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

public class DepartmentEmployeesGet : EndpointBaseAsync
    .WithRequest<DepartmentEmployeesGetRequest>
    .WithActionResult<DepartmentEmployeesResponse>
{
    private readonly IQueryDispatcher dispatcher;

    public DepartmentEmployeesGet(IQueryDispatcher dispatcher)
    {
        Guard.Against.Null(dispatcher, nameof(dispatcher));

        this.dispatcher = dispatcher;
    }

    [HttpGet("departments/employees"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Gets department employees",
        Description = "Gets all employees matching request parameters in the given department",
        OperationId = "DepartmentEmployees.GetAll",
        Tags = new[] { "Employees", "Departments" })
    ]
    public override async Task<ActionResult<DepartmentEmployeesResponse>> HandleAsync([FromQuery] DepartmentEmployeesGetRequest request, CancellationToken token) =>
         (await Result.Try(() => dispatcher.Dispatch(new DepartmentEmployeesQuery(request.Count, request.Department), token)))
            .Match<IEnumerable<DepartmentEmployee>, ActionResult<DepartmentEmployeesResponse>>(
                e => new DepartmentEmployeesResponse(e.GetValueOrDefault()),
                () => NotFound(),
                ex => BadRequest(ex));
}

public class DepartmentEmployeesGetRequest
{
    public int Count { get; set; }
    public string Department { get; set; } = "";
}

public class DepartmentEmployeesResponse : IListResponse<DepartmentEmployee>
{
    public IEnumerable<DepartmentEmployee> Data { get; }

    public DepartmentEmployeesResponse(IEnumerable<DepartmentEmployee> data)
    {
        Guard.Against.Null(data, nameof(data));

        Data = data;
    }
}
