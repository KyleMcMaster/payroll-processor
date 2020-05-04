using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PayrollProcessor.Functions.Features.Employees;
using PayrollProcessor.Functions.Features.Payrolls;
using PayrollProcessor.Infrastructure.Seeding.Features.Employees;
using PayrollProcessor.Infrastructure.Seeding.Features.Generators;

using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Resources
{
    public class ResourcesTrigger
    {
        private readonly CosmosClient client;

        public ResourcesTrigger(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        [FunctionName(nameof(CreateResources))]
        public async Task<ActionResult> CreateResources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "resources")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Creating all tables and queues: [{req}]");

            var dbResponse = await client.CreateDatabaseIfNotExistsAsync(Databases.PayrollProcessor.Name);

            await dbResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(Databases.PayrollProcessor.Containers.Employees, partitionKeyPath: "/employeeId"));
            await dbResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(Databases.PayrollProcessor.Containers.Payrolls, partitionKeyPath: "/checkDate"));

            return new OkResult();
        }

        [FunctionName(nameof(DeleteAllResources))]
        public async Task<ActionResult> DeleteAllResources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "resources")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Deleting all tables and queues: [{req}]");

            var db = client.GetDatabase(Databases.PayrollProcessor.Name);

            await db.DeleteAsync();

            return new OkResult();
        }

        [FunctionName(nameof(CreateData))]
        public async Task<ActionResult> CreateData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "resources/data")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Creating all seed data: [{req}]");

            req.Query.TryGetValue("employeesCount", out var employeesCountQuery);
            req.Query.TryGetValue("payrollsMaxCount", out var payrollsMaxCountQuery);

            int employeesCount = int.Parse(employeesCountQuery.FirstOrDefault() ?? "5");
            int payrollsMaxCount = int.Parse(payrollsMaxCountQuery.FirstOrDefault() ?? "10");

            var domainSeed = new DomainSeed(new EmployeeSeed());

            var employeesContainer = client.GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Employees);
            var payrollsContainer = client.GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Payrolls);

            foreach (var employee in domainSeed.BuildAll(employeesCount, payrollsMaxCount))
            {
                var employeeEntity = EmployeeEntity.Map.From(employee);

                var response = await employeesContainer.CreateItemAsync(employeeEntity);

                foreach (var payroll in employee.Payrolls)
                {
                    var payrollEntity = PayrollEntity.Map.From(employee, payroll);
                    var employeePayroll = EmployeePayrollEntity.Map.From(payroll);

                    await payrollsContainer.CreateItemAsync(payrollEntity);
                    await employeesContainer.CreateItemAsync(employeePayroll);
                }
            }

            return new OkResult();
        }
    }
}
