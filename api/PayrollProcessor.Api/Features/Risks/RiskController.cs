using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayrollProcessor.Data.Domain.Features;
using PayrollProcessor.Data.Persistence.Context;
using System.Collections.Generic;
using System.Linq;

namespace PayrollProcessor.Api.Features.Risks
{
    [ApiController]
    [Route("risk")]
    public class RiskController : ControllerBase
    {
        private readonly IDbContext dbContext;
        private readonly ILogger<RiskController> logger;
        private readonly IMapper mapper;

        public RiskController(IDbContext dbContext, ILogger<RiskController> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Risk> Get()
        {
            var risks = dbContext.Risks.AsEnumerable();

            return mapper.Map<IEnumerable<Risk>>(risks);
        }
    }
}
