using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class UserGetByEmailUserCase : BaseAdminUseCase, IUserGetByEmailUserCase
    {
        public User GetUser(string username)
        {
            return Context.UserContext.UserRepository.GetByUsername(username);
        }
    }
}
