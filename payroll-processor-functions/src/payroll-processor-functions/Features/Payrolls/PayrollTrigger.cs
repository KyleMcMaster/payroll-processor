using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Features.Employees;
using PayrollProcessor.Functions.Features.Resources;
using PayrollProcessor.Functions.Infrastructure;
using System;
using System.Threading.Tasks;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public class PayrollTrigger
    {
        [FunctionName(nameof(GetPayrolls))]
        public async Task<IActionResult> GetPayrolls(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "payrolls")] HttpRequest req,
            [Table(Resource.Table.Payrolls)] CloudTable payrollesTable,
            ILogger log)
        {
            log.LogInformation($"Retrieving all payrolls: [{req}]");

            var payrollQuerier = new TableQuerier(payrollesTable);

            var payrolls = await payrollQuerier.GetAllData<Payroll, PayrollEntity>(e => PayrollEntity.Map.To(e));

            return new OkObjectResult(Response.Generate(payrolls));
        }

        [FunctionName(nameof(GetAllPayrollsForEmployee))]
        public async Task<IActionResult> GetAllPayrollsForEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees/{employeeId}/payrolls")] HttpRequest req,
            [Table(Resource.Table.EmployeePayrolls)] CloudTable employeePayrollsTable,
            Guid employeeId,
            ILogger log)
        {
            log.LogInformation($"Retrieving all payrolls for employee [{employeeId}]: [{req}]");

            var payrollQuerier = new TableQuerier(employeePayrollsTable);

            var payrolls = await payrollQuerier
                .GetAllDataByPartitionKey<Payroll, EmployeePayrollEntity>(
                    e => EmployeePayrollEntity.Map.To(e),
                    employeeId.ToString("n"));

            return new OkObjectResult(Response.Generate(payrolls));
        }

        [FunctionName(nameof(CreatePayroll))]
        public async Task<IActionResult> CreatePayroll(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "payrolls")] HttpRequest req,
                [Table(Resource.Table.Employees)] CloudTable employeeTable,
                [Table(Resource.Table.Payrolls)] CloudTable payrollsTable,
                [Queue(Resource.Queue.PayrollUpdates)] CloudQueue payrollUpdatesQueue,
                ILogger log)
        {
            log.LogInformation($"Creating a new payroll: [{req}]");

            var payroll = await Request.Parse<Payroll>(req);

            payroll.Id = Guid.NewGuid();

            var querier = new TableQuerier(employeeTable);

            var option = await querier.GetEntity<EmployeeEntity, Employee>(
                payroll.EmployeeDepartment.ToLowerInvariant(),
                payroll.EmployeeId.ToString("n"),
                EmployeeEntity.Map.To);

            var employee = option.IfNone(() => throw new Exception($"Could not find employee [{payroll.EmployeeDepartment}] [{payroll.EmployeeId}]"));

            var tableResult = await payrollsTable.ExecuteAsync(TableOperation.Insert(PayrollEntity.Map.From(payroll)));

            if (!(tableResult.Result is PayrollEntity payrollEntity))
            {
                throw new Exception($"Could not save payroll");
            }

            await payrollUpdatesQueue.AddMessageAsync(EntityQueueMessageProcessor.ToQueueMessage(payrollEntity));

            payroll = PayrollEntity.Map.To(payrollEntity);

            return new OkObjectResult(payroll);
        }

        [FunctionName(nameof(UpdatePayroll))]
        public async Task<IActionResult> UpdatePayroll(
                [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "payrolls")] HttpRequest req,
                [Table(Resource.Table.Payrolls)] CloudTable payrollsTable,
                [Queue(Resource.Queue.PayrollUpdates)] CloudQueue payrollUpdatesQueue,
                ILogger log)
        {
            log.LogInformation($"Creating a new payroll: [{req}]");

            var payroll = await Request.Parse<Payroll>(req);

            var payrollEntity = PayrollEntity.Map.From(payroll);

            var payrollUpdate = TableOperation.Replace(payrollEntity);

            await payrollsTable.ExecuteAsync(payrollUpdate);

            await payrollUpdatesQueue.AddMessageAsync(EntityQueueMessageProcessor.ToQueueMessage(payrollEntity));

            return new OkObjectResult(payroll);
        }

        [FunctionName(nameof(UpdatePayrollFromQueue))]
        public async Task UpdatePayrollFromQueue(
                [QueueTrigger(Resource.Queue.PayrollUpdates)] CloudQueueMessage message,
                [Table(Resource.Table.Employees)] CloudTable employeeTable,
                [Table(Resource.Table.EmployeePayrolls)] CloudTable employeePayrollsTable,
                ILogger log)
        {
            log.LogInformation($"Processing payrollUpdates queue: [{message}]");

            var payrollEntity = EntityQueueMessageProcessor.FromQueueMessage<PayrollEntity>(message);

            var payroll = PayrollEntity.Map.To(payrollEntity);

            var querier = new TableQuerier(employeeTable);

            var option = await querier.GetEntity<EmployeeEntity, Employee>(
                payrollEntity.EmployeeDepartment.ToLowerInvariant(),
                payrollEntity.EmployeeId.ToString("n"),
                EmployeeEntity.Map.To);

            var employee = option.IfNone(() => throw new Exception($"Could not find employee for payroll [{payroll.Id}]"));

            employee
                .Payrolls
                .Find(p => p.Id == payroll.Id)
                .Match(p =>
                    {
                        p.GrossPayroll = payroll.GrossPayroll;
                        p.PayrollPeriod = p.PayrollPeriod;
                    },
                    () =>
                    {
                        employee.Payrolls = employee.Payrolls.Length() < 30
                            ? employee.Payrolls.Append(new[] { new EmployeePayroll { Id = payroll.Id, CheckDate = payroll.CheckDate, GrossPayroll = payroll.GrossPayroll, PayrollPeriod = payroll.PayrollPeriod } })
                            : employee.Payrolls;
                    });

            var employeeUpdate = TableOperation.Replace(EmployeeEntity.Map.From(employee));

            await employeeTable.ExecuteAsync(employeeUpdate);

            var employeePayrollUpdate = TableOperation.InsertOrReplace(EmployeePayrollEntity.Map.From(payroll));

            await employeePayrollsTable.ExecuteAsync(employeePayrollUpdate);
        }
    }
}
