using CoachSeek.Application.Contracts.Models.Responses;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetByDomainUseCase
    {
        BusinessGetResponse GetByDomain(string domain);
    }
}
