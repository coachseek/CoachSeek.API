using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.DataAccess.Authentication.Conversion;
using CoachSeek.DataAccess.Authentication.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Authentication.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        // Spy behaviour is included
        public bool WasSaveNewUserCalled;
        public bool WasSaveUserCalled;

        public static List<DbUser> Users { get; private set; }


        static InMemoryUserRepository()
        {
            Users = new List<DbUser>();
        }

        public void Clear()
        {
            Users.Clear();
        }

        public void Save(NewUser newUser)
        {
            WasSaveNewUserCalled = true;

            var dbUser = DbUserConverter.Convert(newUser);

            Users.Add(dbUser);
        }

        public void Save(User user)
        {
            WasSaveUserCalled = true;

            var dbUser = DbUserConverter.Convert(user);
            var existingUser = Users.Single(x => x.Id == dbUser.Id);
            var existingIndex = Users.IndexOf(existingUser);
            Users[existingIndex] = dbUser;
        }

        public User Get(Guid id)
        {
            var dbUser = Users.FirstOrDefault(x => x.Id == id);
            return CreateUser(dbUser);
        }

        public User GetByUsername(string username)
        {
            var dbUser = Users.FirstOrDefault(x => x.Username == username);
            return CreateUser(dbUser);
        }

        public User GetByBusinessId(Guid businessId)
        {
            throw new NotImplementedException();
        }


        private User CreateUser(DbUser dbUser)
        {
            if (dbUser == null)
                return null;

            return new User(dbUser.Id,
                dbUser.BusinessId,
                dbUser.BusinessName,
                dbUser.Email,
                dbUser.FirstName,
                dbUser.LastName,
                dbUser.Username,
                dbUser.PasswordHash);
        }


        // Only used for tests to add a user while bypassing the validation that occurs using Save.
        public void Add(User user)
        {
            var dbUser = DbUserConverter.Convert(user);
            Users.Add(dbUser);
        }

        public void Add(IEnumerable<User> users)
        {
            foreach (var user in users)
                Add(user);
        }
    }
}
