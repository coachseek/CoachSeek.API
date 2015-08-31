using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class UserContext
    {
        public UserDetails User { get; private set; }
        public IUserRepository UserRepository { get; private set; }


        public UserContext(UserDetails currentUser, IUserRepository userRepository)
        {
            User = currentUser;
            UserRepository = userRepository;
        }

        //private string ResolveUsername(string userName, IUserRepository userRepository)
        //{
        //    if (!IsUserInBusinessContext(userName))
        //        return null;
        //    if (IsAuthenticatedUser(userName))
        //        return userName;
        //    var user = LookupBusinessAdminUser(userRepository);
        //    return user.Email;
        //}

        //private bool IsUserInBusinessContext(string userName)
        //{
        //    if (Business == null)
        //        return false;
        //    return Business.Id != Guid.Empty && userName != "";
        //}

        //private bool IsAuthenticatedUser(string userName)
        //{
        //    return userName != Constants.ANONYMOUS_USER;
        //}

        //private User LookupBusinessAdminUser(IUserRepository userRepository)
        //{
        //    var user = userRepository.GetByBusinessId(Business.Id);
        //    if (user.IsNotFound())
        //        throw new InvalidOperationException("User not associated with a business.");
        //    return user;
        //}
    }
}
