using NSubstitute;
using Payroll.Processor.Data.Persistence.Context;
using Xunit;

namespace Payroll.Processor.Data.Persistence.Tests.Features.Risks
{
    public class RiskTests
    {
        private IDbContext DbContext { get; }

        public RiskTests()
        {
            var dataSeed = new DbContextDataSeed();

            DbContext = Substitute.For<IDbContext>();

            DbContext.Risks.Returns(dataSeed.Risks);
        }

        [Fact]
        public void Test1()
        {
        }
    }
}
