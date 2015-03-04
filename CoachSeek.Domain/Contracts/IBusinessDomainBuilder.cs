
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Contracts
{
    public interface IBusinessDomainBuilder
    {
        IBusinessRepository BusinessRepository { get; set; }

        string BuildDomain(string businessName);
    }
}
