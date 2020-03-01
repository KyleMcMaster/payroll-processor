using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayrollProcessor.Functions.Features.Employee;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollProcessor.Functions.Features.Payroll
{
    public static class PayrollGetTrigger
    {
        [FunctionName("PayrollGetTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string department = req.Query["department"];

            var payrollDetails = EmployeeData.Employees()
                .Join(PayrollData.Payrolls()
                , e => e.Id
                , p => p.EmployeeId
                , (e, p) => new EmployeePayrollResponse
                {
                    Id = p.Id,
                    CheckDate = p.CheckDate,
                    EmployeeDepartment = e.Department,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = $"{e.FirstName} {e.LastName}",
                    GrossPayroll = p.GrossPayroll
                });

            if (!string.IsNullOrWhiteSpace(department))
            {
                payrollDetails = payrollDetails.Where(pd => pd.EmployeeDepartment == department);
            }

            payrollDetails = payrollDetails.OrderByDescending(ep => ep.CheckDate);

            string responseMessage = JsonConvert.SerializeObject(
                value: payrollDetails,
                settings: DefaultJsonSerializerSettings.JsonSerializerSettings);

            return new OkObjectResult(responseMessage);
        }
    }
}
