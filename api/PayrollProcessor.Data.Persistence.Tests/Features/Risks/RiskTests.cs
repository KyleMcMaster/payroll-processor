using NSubstitute;
using PayrollProcessor.Data.Persistence.Context;
using Xunit;

namespace PayrollProcessor.Data.Persistence.Tests.Features.Risks
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
