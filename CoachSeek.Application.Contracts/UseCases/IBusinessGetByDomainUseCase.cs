using CoachSeek.Application.Contracts.Models.Responses;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetByDomainUseCase
    {
        Response GetByDomain(string domain);
    }
}
