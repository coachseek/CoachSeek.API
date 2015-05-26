using System;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IUserRepository
    {
        User Save(NewUser newUser);
        User Save(User user);

        User Get(Guid id);
        User GetByUsername(string username);
        User GetByBusinessId(Guid businessId);
    }
}
