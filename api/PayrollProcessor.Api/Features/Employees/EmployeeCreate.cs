using System;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Api.Features.Employees
{
    public class EmployeeCreate : BaseAsyncEndpoint<EmployeeCreateRequest, EmployeeCreateResponse>
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

        [HttpGet("employees"), MapToApiVersion("1")]
        [SwaggerOperation(
            Summary = "Creates a new employee",
            Description = "Creates a new employee and returns the entity's id",
            OperationId = "Employees.Create",
            Tags = new[] { "Employees" })
        ]
        public override Task<ActionResult<EmployeeCreateResponse>> HandleAsync([FromBody] EmployeeCreateRequest request)
        {
            var command = new EmployeeCreateCommand(
                generator.Generate(),
                new EmployeeNew
                {
                    Department = request.Department,
                    Email = request.Email,
                    EmploymentStartedOn = request.EmploymentStartedOn,
                    FirstName = request.FirstName,
                    LastName = request.FirstName,
                    Phone = request.Phone,
                    Status = request.Status,
                    Title = request.Title
                });

            return dispatcher
                .Dispatch(command)
                .ToAsync()
                .Match<ActionResult<EmployeeCreateResponse>>(
                    _ => new EmployeeCreateResponse(command.NewId),
                    ex => BadRequest(ex.Message));
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

    public class EmployeeCreateResponse
    {
        public Guid EmployeeId { get; }

        public EmployeeCreateResponse(Guid employeeId)
        {
            Guard.Against.Default(employeeId, nameof(employeeId));

            EmployeeId = employeeId;
        }
    }
}
