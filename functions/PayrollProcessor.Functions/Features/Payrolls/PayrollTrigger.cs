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
using System.Linq;
using PayrollProcessor.Functions.Domain.Features.Payrolls;
using PayrollProcessor.Functions.Domain.Features.Employees;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public class PayrollTrigger
    {
        private readonly ApiClient apiClient;

        public PayrollTrigger(ApiClient apiClient)
        {
            if (apiClient is null)
            {
                throw new ArgumentNullException(nameof(apiClient));
            }

            this.apiClient = apiClient;
        }

        [FunctionName(nameof(GetPayrolls))]
        public async Task<ActionResult<Payroll[]>> GetPayrolls(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "payrolls")] HttpRequest req,
            [Table(Resource.Table.Payrolls)] CloudTable payrollesTable,
            ILogger log)
        {
            log.LogInformation($"Retrieving all payrolls: [{req}]");

            var payrollQuerier = new TableQuerier(payrollesTable);

            var payrolls = await payrollQuerier.GetAllData<Payroll, PayrollEntity>(e => PayrollEntity.Map.To(e));

            return payrolls.ToArray();
        }

        [FunctionName(nameof(GetAllPayrollsForEmployee))]
        public async Task<ActionResult<Payroll[]>> GetAllPayrollsForEmployee(
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

            return payrolls.ToArray();
        }

        [FunctionName(nameof(CreatePayroll))]
        public async Task<ActionResult<Payroll>> CreatePayroll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "payrolls")] HttpRequest req,
            [Table(Resource.Table.Employees)] CloudTable employeeTable,
            [Table(Resource.Table.Payrolls)] CloudTable payrollsTable,
            [Queue(Resource.Queue.PayrollUpdates)] CloudQueue payrollUpdatesQueue,
            ILogger log)
        {
            log.LogInformation($"Creating a new payroll: [{req}]");

            var payrollNew = await Request.Parse<PayrollNew>(req);

            var querier = new TableQuerier(employeeTable);

            var option = await querier.GetEntity<EmployeeEntity, Employee>(
                payrollNew.EmployeeDepartment.ToLowerInvariant(),
                payrollNew.EmployeeId.ToString("n"),
                EmployeeEntity.Map.ToEmployee);

            var employee = option.IfNone(() => throw new Exception($"Could not find employee [{payrollNew.EmployeeDepartment}] [{payrollNew.EmployeeId}]"));

            var tableResult = await payrollsTable.ExecuteAsync(TableOperation.Insert(PayrollEntity.Map.From(payrollNew)));

            if (!(tableResult.Result is PayrollEntity payrollEntity))
            {
                throw new Exception($"Could not save payroll");
            }

            await payrollUpdatesQueue.AddMessageAsync(EntityQueueMessageProcessor.ToQueueMessage(payrollEntity));

            return PayrollEntity.Map.To(payrollEntity);
        }

        [FunctionName(nameof(UpdatePayroll))]
        public async Task<ActionResult<Payroll>> UpdatePayroll(
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

            return payroll;
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
                EmployeeEntity.Map.ToEmployee);

            var employee = option.IfNone(() => throw new Exception($"Could not find employee for payroll [{payroll.Id}]"));

            employee
                .Payrolls
                .Find(p => p.Id == payroll.Id)
                .Match(p =>
                    {
                        p.GrossPayroll = payroll.GrossPayroll;
                        p.PayrollPeriod = p.PayrollPeriod;
                    },
                    () => employee.UpdatePayrolls(payroll));

            var employeeUpdate = TableOperation.Replace(EmployeeEntity.Map.From(employee));

            await employeeTable.ExecuteAsync(employeeUpdate);

            var employeePayrollUpdate = TableOperation.InsertOrReplace(EmployeePayrollEntity.Map.From(payroll));

            await apiClient.SendNotification(
                nameof(UpdatePayrollFromQueue),
                payroll
            );

            await employeePayrollsTable.ExecuteAsync(employeePayrollUpdate);
        }
    }
}
