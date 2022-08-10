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

public class EmployeePayrollUpdate : EndpointBaseAsync
    .WithRequest<EmployeePayrollUpdateRequest>
    .WithActionResult
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
    public override Task<ActionResult> HandleAsync(EmployeePayrollUpdateRequest request, CancellationToken token)
    {
        return queryDispatcher.Dispatch(new EmployeePayrollQuery(request.EmployeeId, request.Id), token)
            .ToResult("Not Found")
            .Bind(UpdateEmployeePayroll)
            .Match(OnSuccess, OnFailure);

        async Task<Result<EmployeePayroll>> UpdateEmployeePayroll(EmployeePayroll employeePayroll)
        {
            var command = new EmployeePayrollUpdateCommand(
                    request.CheckDate,
                    request.GrossPayroll,
                    request.PayrollPeriod,
                    employeePayroll
                );

            return await commandDispatcher.Dispatch(command);
        }
    }
    private ActionResult OnSuccess(EmployeePayroll employeePayroll) => Ok(employeePayroll);
    private ActionResult OnFailure(string errorMessage) => new APIErrorResult(errorMessage);
}

public class EmployeePayrollUpdateRequest
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTimeOffset CheckDate { get; set; }
    public decimal GrossPayroll { get; set; }
    public string PayrollPeriod { get; set; } = "";
}
