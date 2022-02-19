using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Employees;

public class EmployeeCreate : EndpointBaseAsync
    .WithRequest<EmployeeCreateRequest>
    .WithActionResult<Employee>
{
    private readonly ICommandDispatcher dispatcher;
    private readonly IEntityIdGenerator generator;

    public EmployeeCreate(ICommandDispatcher dispatcher, IEntityIdGenerator generator)
    {
        Guard.Against.Null(dispatcher, nameof(dispatcher));
        Guard.Against.Null(generator, nameof(generator));

        this.dispatcher = dispatcher;
        this.generator = generator;
    }

    [HttpPost("employees"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Creates a new employee",
        Description = "Creates a new employee and returns the entity's id",
        OperationId = "Employees.Create",
        Tags = new[] { "Employees" })
    ]
    public override Task<ActionResult<Employee>> HandleAsync([FromBody] EmployeeCreateRequest request, CancellationToken token)
    {
        var command = new EmployeeCreateCommand(
            generator.Generate(),
            new EmployeeNew
            {
                Department = request.Department,
                Email = request.Email,
                EmploymentStartedOn = request.EmploymentStartedOn,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Status = request.Status,
                Title = request.Title
            });

        return dispatcher
            .Dispatch(command)
            .Match<Employee, ActionResult<Employee>>(
                employee => employee,
                ex => new APIErrorResult(ex.Message));
    }
}

public class EmployeeCreateRequest
{
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset EmploymentStartedOn { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
