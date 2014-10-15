using CoachSeek.Services.Contracts.Builders;

namespace CoachSeek.Domain.Tests.Unit.Fakes
{
    public class MockBusinessDomainBuilder : IBusinessDomainBuilder
    {
        public bool WasBuildDomainCalled;
        public string Domain;

        public string BuildDomain(string businessName)
        {
            WasBuildDomainCalled = true;

            return Domain;
        }
    }
}
