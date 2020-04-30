using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.Processor.Data.Domain.Features.Employees;
using Payroll.Processor.Data.Persistence.Context;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Processor.Api.Features.Employees
{
    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IDbContext dbContext;
        private readonly ILogger<EmployeeController> logger;
        private readonly IMapper mapper;

        public EmployeeController(IDbContext dbContext, ILogger<EmployeeController> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            var employees = dbContext.Employees.AsEnumerable();

            return mapper.Map<IEnumerable<Employee>>(employees);
        }
    }
}
