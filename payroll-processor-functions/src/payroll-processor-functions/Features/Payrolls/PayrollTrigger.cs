using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PayrollProcessor.Functions.Features.Employees;
using PayrollProcessor.Functions.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public class PayrollTrigger
    {
        public const string CONNECTION = "AzureTableStorage";

        [FunctionName("Payroll_Get")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "payrolls")] HttpRequest req,
            [Table("employees", Connection = CONNECTION)] CloudTable employeeTable,
            [Table("payrolls", Connection = CONNECTION)] CloudTable payrollesTable,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request [{req}]");

            var employeeQuerier = new TableQuerier(employeeTable);

            var employees = await employeeQuerier.GetAllData<Employee, EmployeeEntity>(e => EmployeeEntity.Map.To(e));

            var payrollQuerier = new TableQuerier(payrollesTable);

            var payrolls = await payrollQuerier.GetAllData<Payroll, PayrollEntity>(e => PayrollEntity.Map.To(e));

            var payrollDetails = employees
                .Join(payrolls
                , e => e.Id
                , p => p.EmployeeId
                , (e, p) => new EmployeePayrollResponse
                {
                    Id = p.Id,
                    CheckDate = p.CheckDate,
                    EmployeeDepartment = e.Department,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = $"{e.FirstName} {e.LastName}",
                    EmployeeStatus = e.Status,
                    GrossPayroll = p.GrossPayroll,
                    PayrollPeriod = p.PayrollPeriod
                });

            payrollDetails = await Task.FromResult(payrollDetails.OrderBy(ep => ep.PayrollPeriod));

            string responseMessage = JsonConvert.SerializeObject(
                value: payrollDetails,
                settings: DefaultJsonSerializerSettings.JsonSerializerSettings);

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("Payroll_Create")]
        [return: Table("payrolls", Connection = CONNECTION)]
        public async Task<PayrollEntity> CreatePayroll(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "payrolls")] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"Creating a new payroll: [{req}]");

            var payroll = await Request.Parse<Payroll>(req);

            return PayrollEntity.Map.From(payroll);
        }

        [FunctionName("PayrollsTable_Create")]
        public async Task<IActionResult> CreatePayrollsTable(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "payrolls/table")] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"Creating payrolls table: [{req}]");

            var tableManager = new TableManager();

            await tableManager.CreateTable("payrolls");

            return new OkResult();
        }
    }
}
