using System;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IUserRepository
    {
        void Save(NewUser newUser);
        void Save(User user);

        User Get(Guid id);
        User GetByUsername(string username);
        User GetByBusinessId(Guid businessId);
    }
}
