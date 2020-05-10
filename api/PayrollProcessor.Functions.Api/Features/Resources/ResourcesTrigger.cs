using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PayrollProcessor.Data.Persistence.Features.Departments;
using PayrollProcessor.Data.Persistence.Features.Employees;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;
using PayrollProcessor.Infrastructure.Seeding.Features.Employees;
using PayrollProcessor.Infrastructure.Seeding.Features.Generators;

using static PayrollProcessor.Data.Persistence.Infrastructure.Clients.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Api.Features.Resources
{
    public class ResourcesTrigger
    {
        private readonly CosmosClient client;

        public ResourcesTrigger(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        [FunctionName(nameof(CreateResources))]
        public async Task<ActionResult> CreateResources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "resources")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Creating all databases, collections, and queues: [{req}]");

            var resourceManager = new ResourceManager();

            await resourceManager.CreateQueue(AppResources.Queue.EmployeeUpdates);
            await resourceManager.CreateQueue(AppResources.Queue.EmployeePayrollUpdates);

            var dbResponse = await client.CreateDatabaseIfNotExistsAsync(Databases.PayrollProcessor.Name);

            await dbResponse.Database.CreateContainerIfNotExistsAsync(
                new ContainerProperties(Databases.PayrollProcessor.Containers.Employees, partitionKeyPath: "/partitionKey"));
            await dbResponse.Database.CreateContainerIfNotExistsAsync(
                new ContainerProperties(Databases.PayrollProcessor.Containers.Departments, partitionKeyPath: "/partitionKey"));

            return new OkResult();
        }

        [FunctionName(nameof(DeleteAllResources))]
        public async Task<ActionResult> DeleteAllResources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "resources")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Deleting all databases, collections, and queues: [{req}]");

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
            var departmentsContainer = client.GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Departments);

            foreach (var employee in domainSeed.BuildAll(employeesCount, payrollsMaxCount))
            {
                var employeeRecord = EmployeeRecord.Map.From(employee);
                var departmentEmployeeRecord = DepartmentEmployeeRecord.Map.CreateNewFrom(employee, Guid.NewGuid());

                await employeesContainer.CreateItemAsync(employeeRecord);
                await departmentsContainer.CreateItemAsync(departmentEmployeeRecord);

                foreach (var payroll in employee.Payrolls)
                {
                    var departmentPayrollRecord = DepartmentPayrollRecord.Map.CreateNewFrom(employee, Guid.NewGuid(), payroll);
                    var employeePayrollRecord = EmployeePayrollRecord.Map.From(payroll);

                    await departmentsContainer.CreateItemAsync(departmentPayrollRecord);
                    await employeesContainer.CreateItemAsync(employeePayrollRecord);
                }
            }

            return new OkResult();
        }
    }
}
