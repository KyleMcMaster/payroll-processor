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
            DbContext = Substitute.For<IDbContext>();

            DbContext.Risks.Returns(DataSeed.Risks());
        }

        [Fact]
        public void Test1()
        {
        }
    }
}
