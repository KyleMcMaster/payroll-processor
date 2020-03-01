using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PayrollProcessor.Functions.Features.Employee
{
    public class EmployeesGetTrigger
    {
        [FunctionName("EmployeesGetTrigger")]
        public async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
                ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = JsonConvert.SerializeObject(
                value: EmployeeData.Employees(),
                settings: DefaultJsonSerializerSettings.JsonSerializerSettings);

            return new OkObjectResult(responseMessage);
        }
    }
}
