using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetByDomainUseCase
    {
        Response<BusinessData> GetByDomain(string domain);
    }
}
