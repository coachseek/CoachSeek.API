using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetByDomainUseCase
    {
        BusinessData GetByDomain(string domain);
    }
}
