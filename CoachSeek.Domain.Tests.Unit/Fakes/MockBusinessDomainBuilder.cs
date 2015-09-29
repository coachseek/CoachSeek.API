using System.Threading.Tasks;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Tests.Unit.Fakes
{
    public class MockBusinessDomainBuilder : IBusinessDomainBuilder
    {
        public bool WasBuildDomainCalled;
        public string Domain;

        public IBusinessRepository BusinessRepository { get; set; }

        public async Task<string >BuildDomainAsync(string businessName)
        {
            WasBuildDomainCalled = true;

            return Domain;
        }
    }
}
