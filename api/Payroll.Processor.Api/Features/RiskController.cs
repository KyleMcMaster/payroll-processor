using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using static Payroll.Processor.Data.Domain.Features.Risks;

namespace Payroll.Processor.Api.Features
{
    [ApiController]
    [Route("risk")]
    public class RiskController : ControllerBase
    {
        private readonly ILogger<RiskController> logger;

        public RiskController(ILogger<RiskController> logger) => this.logger = logger;


        [HttpGet]
        public IEnumerable<Risk> Get()
        {
            return new Risk[] { Risk.LOW, Risk.MEDIUM, Risk.HIGH };
        }
    }
}