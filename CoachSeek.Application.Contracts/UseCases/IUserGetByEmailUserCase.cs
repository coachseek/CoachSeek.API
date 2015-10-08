using System.Threading.Tasks;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserGetByEmailUserCase : IAdminApplicationContextSetter
    {
        Task<User> GetUserAsync(string email);
    }
}
