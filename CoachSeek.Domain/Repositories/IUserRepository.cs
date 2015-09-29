using System;
using System.Threading.Tasks;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IUserRepository
    {
        Task SaveAsync(NewUser newUser);
        void Save(NewUser newUser);
        Task SaveAsync(User user);
        void Save(User user);

        Task<User> GetAsync(Guid id);
        User Get(Guid id);
        Task<User> GetByUsernameAsync(string username);
        User GetByUsername(string username);
        User GetByBusinessId(Guid businessId);
    }
}
