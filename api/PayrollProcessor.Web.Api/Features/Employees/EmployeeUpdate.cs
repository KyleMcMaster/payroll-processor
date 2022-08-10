using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Employees;

public class EmployeeUpdate : EndpointBaseAsync
    .WithRequest<EmployeeUpdateRequest>
    .WithActionResult
{
    private readonly ICommandDispatcher commandDispatcher;
    private readonly IQueryDispatcher queryDispatcher;

    public EmployeeUpdate(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));
        Guard.Against.Null(commandDispatcher, nameof(commandDispatcher));

        this.queryDispatcher = queryDispatcher;
        this.commandDispatcher = commandDispatcher;
    }

    [HttpPut("employees"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Updates employee",
        Description = "Updates the employee with the request parameters",
        OperationId = "Employee.Update",
        Tags = new[] { "Employees" })
    ]
    public override Task<ActionResult> HandleAsync(EmployeeUpdateRequest request, CancellationToken token) =>
        queryDispatcher.Dispatch(new EmployeeQuery(request.Id), token)
            .ToResult("Not Found")
            .Map(employee => new EmployeeUpdateCommand(
                request.Email,
                request.EmploymentStartedOn,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Status,
                request.Title,
                request.Version,
                employee
            ))
            .Bind(command => commandDispatcher.Dispatch(command))
            .Match(OnSuccess, OnFailure);

    private ActionResult OnSuccess(Employee employee) => Ok(employee);
    private ActionResult OnFailure(string errorMessage) => new APIErrorResult(errorMessage);
}

public class EmployeeUpdateRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; } = "";
    public DateTimeOffset EmploymentStartedOn { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Status { get; set; } = "";
    public string Title { get; set; } = "";
    public string Version { get; set; } = "";
}
