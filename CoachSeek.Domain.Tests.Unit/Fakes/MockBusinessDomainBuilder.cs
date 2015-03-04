using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Tests.Unit.Fakes
{
    public class MockBusinessDomainBuilder : IBusinessDomainBuilder
    {
        public bool WasBuildDomainCalled;
        public string Domain;

        public IBusinessRepository BusinessRepository { get; set; }

        public string BuildDomain(string businessName)
        {
            WasBuildDomainCalled = true;

            return Domain;
        }
    }
}
