using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Api.Features.Employees
{
    public class EmployeesGet : BaseEndpoint<EmployeesGetRequest, EmployeesResponse>
    {
        [HttpGet("employees"), MapToApiVersion("1")]
        [SwaggerOperation(
            Summary = "Gets employees",
            Description = "Gets all employees matching request parameters",
            OperationId = "Employees.GetAll",
            Tags = new[] { "Employees" })
        ]
        public override ActionResult<EmployeesResponse> Handle([FromQuery] EmployeesGetRequest request)
        {
            var response = new EmployeesResponse();

            return response;
        }
    }

    public class EmployeesGetRequest
    {

    }

    public class EmployeesResponse
    {
        public string Hello { get; set; } = "World";
    }
}
