using System;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Api.Features.Employees
{

    public class EmployeeGet : BaseAsyncEndpoint<Guid, Employee>
    {
        private readonly IQueryDispatcher dispatcher;

        public EmployeeGet(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        [HttpGet("employees/{employeeId:Guid}"), MapToApiVersion("1")]
        [SwaggerOperation(
            Summary = "Gets a specific employee",
            Description = "Gets a specific employee specified by the route parameter",
            OperationId = "Employee.Get",
            Tags = new[] { "Employees" })
        ]
        public override Task<ActionResult<Employee>> HandleAsync([FromRoute] Guid employeeId) =>
            dispatcher
                .Dispatch(new EmployeeQuery(employeeId))
                .Match<Employee, ActionResult<Employee>>(
                    e => e,
                    () => NotFound(),
                    ex => new APIErrorResult(ex.Message));
    }
}
