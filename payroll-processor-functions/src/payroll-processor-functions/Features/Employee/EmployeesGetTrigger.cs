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
        [FunctionName(nameof(EmployeesGetTrigger))]
        public async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request: [{req}]");

            var value = await Task.FromResult(EmployeeData.Employees());

            string responseMessage = JsonConvert.SerializeObject(
                value: value,
                settings: DefaultJsonSerializerSettings.JsonSerializerSettings);

            return new OkObjectResult(responseMessage);
        }
    }
}
