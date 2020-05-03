using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Features.Employees;
using PayrollProcessor.Functions.Features.Payrolls;
using PayrollProcessor.Functions.Seeding.Features.Employees;
using PayrollProcessor.Functions.Seeding.Features.Generators;

namespace PayrollProcessor.Functions.Features.Resources
{
    public class ResourcesTrigger
    {
        [FunctionName(nameof(CreateResources))]
        public async Task<ActionResult> CreateResources(
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

        [FunctionName(nameof(DeleteAllResources))]
        public async Task<ActionResult> DeleteAllResources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "resources")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Deleting all tables and queues: [{req}]");

            var manager = new ResourceManager();

            await manager.DeleteTable(Resource.Table.Employees);
            await manager.DeleteTable(Resource.Table.Payrolls);
            await manager.DeleteTable(Resource.Table.EmployeePayrolls);
            await manager.DeleteQueue(Resource.Queue.PayrollUpdates);

            return new OkResult();
        }

        [FunctionName(nameof(CreateData))]
        public async Task<ActionResult> CreateData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "resources/data")] HttpRequest req,
            [Table(Resource.Table.Employees)] CloudTable employeeTable,
            [Table(Resource.Table.Payrolls)] CloudTable payrollsTable,
            [Table(Resource.Table.EmployeePayrolls)] CloudTable employeePayrollsTable,
            ILogger log)
        {
            log.LogInformation($"Creating all seed data: [{req}]");

            req.Query.TryGetValue("employeesCount", out var employeesCountQuery);
            req.Query.TryGetValue("payrollsMaxCount", out var payrollsMaxCountQuery);

            int employeesCount = int.Parse(employeesCountQuery.FirstOrDefault() ?? "5");
            int payrollsMaxCount = int.Parse(payrollsMaxCountQuery.FirstOrDefault() ?? "10");

            var employeeSeed = new EmployeeSeed();

            var domainSeed = new DomainSeed(employeeSeed);

            foreach (var (employee, payrolls) in domainSeed.BuildAll(employeesCount, payrollsMaxCount))
            {
                var employeeTableResult = await employeeTable.ExecuteAsync(TableOperation.Insert(EmployeeEntity.Map.From(employee)));

                if (!(employeeTableResult.Result is EmployeeEntity employeeEntity))
                {
                    throw new Exception($"Could not save employee [{employee.Id}]: {employeeTableResult.HttpStatusCode} {employeeTableResult.Result}");
                }

                foreach (var grouping in payrolls.GroupBy(p => p.CheckDate.ToString("yyyyMMdd")))
                {
                    var payrollsTableBatch = new TableBatchOperation();

                    foreach (var payroll in grouping.Select(g => g))
                    {
                        payrollsTableBatch.Add(TableOperation.Insert(PayrollEntity.Map.From(payroll)));
                    }

                    var payrollsTableResult = await payrollsTable.ExecuteBatchAsync(payrollsTableBatch);

                    if (payrollsTableResult.Count != payrollsTableBatch.Count)
                    {
                        throw new Exception($"Could not save payrolls for employee [{employee.Id}]");
                    }
                }

                var employeePayrollsTableBatch = new TableBatchOperation();

                var mapFunc = EmployeePayrollEntity.Map.From(employee);

                foreach (var employeePayroll in employee.Payrolls)
                {
                    employeePayrollsTableBatch.Add(TableOperation.Insert(mapFunc(employeePayroll)));
                }

                var employeeParollsResult = await employeePayrollsTable.ExecuteBatchAsync(employeePayrollsTableBatch);

                if (employeeParollsResult.Count != employee.Payrolls.Count())
                {
                    throw new Exception($"Could not create employee payrolls for employee [{employee.Id}]");
                }
            }

            return new OkResult();
        }
    }
}
