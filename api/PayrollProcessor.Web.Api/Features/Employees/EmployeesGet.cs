using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Web.Api.Infrastructure.Responses;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Employees
{
    public class EmployeesGet : BaseAsyncEndpoint<EmployeesGetRequest, EmployeesResponse>
    {
        private readonly IQueryDispatcher dispatcher;

        public EmployeesGet(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        [HttpGet("employees"), MapToApiVersion("1")]
        [SwaggerOperation(
            Summary = "Gets employees",
            Description = "Gets all employees matching request parameters",
            OperationId = "Employees.GetAll",
            Tags = new[] { "Employees" })
        ]
        public override Task<ActionResult<EmployeesResponse>> HandleAsync([FromQuery] EmployeesGetRequest request) =>
             dispatcher
                .Dispatch(new EmployeesQuery(request.Count, request.Email, request.FirstName, request.LastName))
                .Match<IEnumerable<Employee>, ActionResult<EmployeesResponse>>(
                    e => new EmployeesResponse(e),
                    () => NotFound("Employees"),
                    ex => BadRequest(ex.Message));
    }

    public class EmployeesGetRequest
    {
        public int Count { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }

    public class EmployeesResponse : IListResponse<Employee>
    {
        public IEnumerable<Employee> Data { get; }

        public EmployeesResponse(IEnumerable<Employee> data)
        {
            Guard.Against.Null(data, nameof(data));

            Data = data;
        }
    }
}
