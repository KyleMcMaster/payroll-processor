using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.Processor.Data.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Processor.Api.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> logger;

        public PaymentController(ILogger<PaymentController> logger) => this.logger = logger;

        [HttpGet]
        [Route("")]
        public IEnumerable<Payment> Get()
        {
            return Enumerable.Empty<Payment>();
        }
    }
}
