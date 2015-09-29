using System.Threading.Tasks;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Contracts
{
    public interface IBusinessDomainBuilder
    {
        IBusinessRepository BusinessRepository { get; set; }

        Task<string> BuildDomainAsync(string businessName);
    }
}
