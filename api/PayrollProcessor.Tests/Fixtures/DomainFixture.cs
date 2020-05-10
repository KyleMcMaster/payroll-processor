using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace PayrollProcessor.Tests.Fixtures
{
    public class DomainFixture : Fixture
    {
        public DomainFixture() : base() =>
            Customize(new AutoNSubstituteCustomization());
    }
}
