using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Api.Infrastructure.Responses;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Api.Features.Employees
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
                .Dispatch(new EmployeesQuery(request.Count, request.FirstName, request.LastName))
                .Match<IEnumerable<Employee>, ActionResult<EmployeesResponse>>(
                    e => new EmployeesResponse(e),
                    () => NotFound(),
                    ex => BadRequest(ex.Message));
    }

    public class EmployeesGetRequest
    {
        public int Count { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }

    public class EmployeesResponse : IListResponse<Employee>
    {
        public static EmployeesResponse Empty = new EmployeesResponse(Enumerable.Empty<Employee>());

        public IEnumerable<Employee> Data { get; }

        public EmployeesResponse(IEnumerable<Employee> data)
        {
            Guard.Against.Null(data, nameof(data));

            Data = data;
        }
    }
}
