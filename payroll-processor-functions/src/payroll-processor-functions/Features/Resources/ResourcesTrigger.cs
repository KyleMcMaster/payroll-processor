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
        [FunctionName(nameof(CreateResources))]
        public async Task<IActionResult> CreateResources(
                        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "resources")] HttpRequest req,
                        ILogger log)
        {
            log.LogInformation($"Creating all tables and queues: [{req}]");

            var manager = new ResourceManager();

            await manager.CreateTable(Resource.Table.Employees);
            await manager.CreateTable(Resource.Table.Payrolls);
            await manager.CreateTable(Resource.Table.EmployeePayrolls);
            await manager.CreateQueue(Resource.Queue.PayrollUpdates);

            return new OkResult();
        }
    }
}
