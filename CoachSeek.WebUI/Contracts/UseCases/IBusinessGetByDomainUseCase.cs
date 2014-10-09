using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.Contracts.UseCases
{
    public interface IBusinessGetByDomainUseCase
    {
        BusinessGetResponse GetByDomain(string domain);
    }
}
