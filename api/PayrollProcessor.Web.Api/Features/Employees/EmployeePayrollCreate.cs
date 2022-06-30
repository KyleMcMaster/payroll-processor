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
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Employees;

public class EmployeePayrollCreate : EndpointBaseAsync
    .WithRequest<EmployeePayrollCreateRequest>
    .WithActionResult
{
    private readonly IQueryDispatcher queryDispatcher;
    private readonly ICommandDispatcher commandDispatcher;
    private readonly IEntityIdGenerator generator;

    public EmployeePayrollCreate(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IEntityIdGenerator generator)
    {
        Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));
        Guard.Against.Null(commandDispatcher, nameof(commandDispatcher));
        Guard.Against.Null(generator, nameof(generator));

        this.commandDispatcher = commandDispatcher;
        this.generator = generator;
        this.queryDispatcher = queryDispatcher;
    }

    [HttpPost("employees/payrolls"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Creates a new employee payroll",
        Description = "Creates a new employee payroll for the specific employee specified by the route parameter",
        OperationId = "EmployeePayroll.Create",
        Tags = new[] { "Employees", "Payrolls" })
    ]
    public override Task<ActionResult> HandleAsync([FromBody] EmployeePayrollCreateRequest request, CancellationToken token)
    {
        return queryDispatcher.Dispatch(new EmployeeQuery(request.EmployeeId), token)
            .ToResult("Not Found")
            .Bind(CreateEmployeePayroll)
            .Match(OnSuccess, OnFailure);

        async Task<Result<EmployeePayroll>> CreateEmployeePayroll(Employee employee)
        {
            var newPayroll = new EmployeePayrollNew
            {
                CheckDate = request.CheckDate,
                GrossPayroll = request.GrossPayroll,
            };

            var newPayrollId = generator.Generate();

            var command = new EmployeePayrollCreateCommand(employee, newPayrollId, newPayroll);

            return await commandDispatcher.Dispatch(command, token);
        }
    }

    private ActionResult OnSuccess(EmployeePayroll employeePayroll) => Ok(employeePayroll);
    private ActionResult OnFailure(string errorMessage) => new APIErrorResult(errorMessage);
}

public class EmployeePayrollCreateRequest
{
    public Guid EmployeeId { get; set; }
    public DateTimeOffset CheckDate { get; set; }
    public decimal GrossPayroll { get; set; }
}
