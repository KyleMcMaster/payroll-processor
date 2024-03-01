using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Employees;

public class EmployeeCreate : EndpointBaseSync
    .WithRequest<EmployeeCreateRequest>
    .WithActionResult<Employee>
{
    private readonly IStranglerCommandDispatcher dispatcher;
    private readonly IEntityIdGenerator generator;

    public EmployeeCreate(IStranglerCommandDispatcher dispatcher, IEntityIdGenerator generator)
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
    public override ActionResult<Employee> Handle([FromBody] EmployeeCreateRequest request)
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
            .Match<ActionResult<Employee>, Employee>(
                onSuccess: employee => Ok(employee),
                onFailure: ex => new APIErrorResult(ex));
    }
}

public class EmployeeCreateRequest
{
    public string Department { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTimeOffset EmploymentStartedOn { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Status { get; set; } = "";
    public string Title { get; set; } = "";
}
