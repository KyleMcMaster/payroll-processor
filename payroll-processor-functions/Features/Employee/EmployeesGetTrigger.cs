using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayrollProcessor.Functions.Features.Seed;
using System.Threading.Tasks;

namespace PayrollProcessor.Functions
{
    public static class EmployeesGetTrigger
    {
        private static JsonSerializerSettings jsonSerializerSettings =>
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        [FunctionName("EmployeesGetTrigger")]
        public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
                ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = JsonConvert.SerializeObject(Data.GetEmployees(), jsonSerializerSettings);

            return new OkObjectResult(responseMessage);
        }
    }
}
