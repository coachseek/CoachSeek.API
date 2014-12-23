using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessAddUseCase
    {
        Response<BusinessData> AddBusiness(BusinessAddCommand command);
    }
}
