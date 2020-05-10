using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace PayrollProcessor.Tests.Fixtures
{
    /// <summary>
    /// Sourced from https://tech.trailmax.info/2014/01/convert-your-projects-from-nunitmoq-to-xunit-with-nsubstitute/
    /// </summary>
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
