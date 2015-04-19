using System;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IUserRepository
    {
        //Task<User> SaveAsync(NewUser newUser);
        User Save(NewUser newUser);
        User Save(User user);

        User Get(Guid id);
        User GetByUsername(string username);
        //Task<User> GetByUsernameAsync(string username);
    }
}
