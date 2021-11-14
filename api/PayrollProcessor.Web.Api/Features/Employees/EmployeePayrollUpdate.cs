using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Employees;

public class EmployeePayrollUpdate : BaseAsyncEndpoint
    .WithRequest<EmployeePayrollUpdateRequest>
    .WithResponse<EmployeePayroll>
{
    private readonly ICommandDispatcher commandDispatcher;
    private readonly IQueryDispatcher queryDispatcher;

    public EmployeePayrollUpdate(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));
        Guard.Against.Null(commandDispatcher, nameof(commandDispatcher));

        this.queryDispatcher = queryDispatcher;
        this.commandDispatcher = commandDispatcher;
    }

    [HttpPut("employees/payrolls"), MapToApiVersion("1")]
    [SwaggerOperation(
        Summary = "Updates employee payroll",
        Description = "Updates the employee payroll with the request parameters",
        OperationId = "EmployeePayroll.Update",
        Tags = new[] { "Employees", "Payrolls" })
    ]
    public override Task<ActionResult<EmployeePayroll>> HandleAsync(EmployeePayrollUpdateRequest request, CancellationToken token) =>
        queryDispatcher.Dispatch(new EmployeePayrollQuery(request.EmployeeId, request.Id), token)
            .Bind(employee =>
            {
                var command = new EmployeePayrollUpdateCommand(
                    request.CheckDate,
                    request.GrossPayroll,
                    request.PayrollPeriod,
                    employee
                );

                return commandDispatcher.Dispatch(command).ToTryOption();
            })
            .Match<EmployeePayroll, ActionResult<EmployeePayroll>>(
                e => e,
                () => NotFound($"Employee payroll [{request.Id}] for employee [{request.EmployeeId}]"),
                ex => new APIErrorResult(ex.Message)
            );
}

public class EmployeePayrollUpdateRequest
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTimeOffset CheckDate { get; set; }
    public decimal GrossPayroll { get; set; }
    public string PayrollPeriod { get; set; } = "";
}
