using System;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Api.Features.Employees
{
    public class EmployeePayrollCreate : BaseAsyncEndpoint<EmployeePayrollCreateRequest, EmployeePayrollCreateResponse>
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

        // [HttpPost("employees/{employeeId:Guid}/payrolls"), MapToApiVersion("1")]
        // [SwaggerOperation(
        //     Summary = "Creates a new employee payroll",
        //     Description = "Creates a new employee payroll for the specific employee specified by the route parameter",
        //     OperationId = "EmployeePayroll.Create",
        //     Tags = new[] { "Employees", "Payrolls" })
        // ]
        // public override Task<ActionResult<EmployeePayrollCreateResponse>> HandleAsync([FromRoute] Guid employeeId, [FromBody] EmployeePayrollCreateRequest request)
        // {
        //     var newPayrollId = generator.Generate();

        //     queryDispatcher
        //         .Dispatch(new EmployeeQuery(employeeId))
        //         .BindAsync()

        //         .BindAsync((
        //             employee =>
        //             {
        //                 var newPayroll = new EmployeePayrollNew
        //                 {
        //                     CheckDate = request.CheckDate,
        //                     GrossPayroll = request.GrossPayroll,
        //                     PayrollPeriod = request.PayrollPeriod
        //                 };

        //                 var command = new EmployeePayrollCreateCommand(employee, newPayrollId, newPayroll);

        //                 return commandDispatcher.Dispatch(command);
        //             })
        //         .ToAsync()
        //         .Match<ActionResult<EmployeePayrollCreateResponse>>(
        //             _ => new EmployeePayrollCreateResponse(newPayrollId),
        //             ex => BadRequest(ex))
        // }
    }

    public class EmployeePayrollCreateRequest
    {
        public DateTimeOffset CheckDate { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
    }

    public class EmployeePayrollCreateResponse
    {
        public Guid EmployeePayrollId { get; }

        public EmployeePayrollCreateResponse(Guid employeePayrollId)
        {
            Guard.Against.Default(employeePayrollId, nameof(employeePayrollId));

            EmployeePayrollId = employeePayrollId;
        }
    }
}
