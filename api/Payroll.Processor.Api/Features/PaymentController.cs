using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payroll.Processor.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Processor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Payment> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Payment
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
