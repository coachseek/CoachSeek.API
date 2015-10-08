using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IUserRepository
    {
        Task SaveAsync(NewUser newUser);
        Task SaveAsync(User user);

        Task<IList<User>> GetAllAsync();
        Task<User> GetAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);
        User GetByBusinessId(Guid businessId);
    }
}
