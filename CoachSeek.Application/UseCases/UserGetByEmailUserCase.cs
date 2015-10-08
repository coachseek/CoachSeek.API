using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class UserGetByEmailUserCase : BaseAdminUseCase, IUserGetByEmailUserCase
    {
        public async Task<User> GetUserAsync(string username)
        {
            return await Context.UserContext.UserRepository.GetByUsernameAsync(username);
        }
    }
}
