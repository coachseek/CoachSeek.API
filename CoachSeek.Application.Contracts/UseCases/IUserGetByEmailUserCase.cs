using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserGetByEmailUserCase : IAdminApplicationContextSetter
    {
        User GetUser(string email);
    }
}
