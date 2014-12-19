using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserAssociateWithBusinessUseCase
    {
        Response<UserData> AssociateUserWithBusiness(UserAssociateWithBusinessCommand command);
    }
}
