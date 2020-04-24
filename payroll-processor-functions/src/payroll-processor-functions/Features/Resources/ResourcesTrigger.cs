using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PayrollProcessor.Functions.Features.Resources
{
    public class ResourcesTrigger
    {
        [FunctionName("Resources_Create")]
        public async Task<IActionResult> CreateEmployeesTable(
                        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "resources")] HttpRequest req,
                        ILogger log)
        {
            log.LogInformation($"Creating all tables and queues: [{req}]");

            var manager = new ResourceManager();

            await manager.CreateTable("employees");
            await manager.CreateTable("payrolls");
            await manager.CreateTable("employeePayrolls");
            await manager.CreateQueue("payroll-updates");

            return new OkResult();
        }
    }
}
