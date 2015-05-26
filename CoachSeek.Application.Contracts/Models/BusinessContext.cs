using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class BusinessContext
    {
        public Guid? BusinessId { get; private set; }
        public string BusinessName { get; private set; }
        public string AdminEmail { get; private set; }
        public IBusinessRepository BusinessRepository { get; private set; }


        public BusinessContext(Guid? businessId, string businessName, IBusinessRepository businessRepository, string userName, IUserRepository userRepository)
        {
            BusinessId = businessId;
            BusinessName = businessName;
            BusinessRepository = businessRepository;
            AdminEmail = ResolveAdminEmail(userName, userRepository);
        }

        private string ResolveAdminEmail(string userName, IUserRepository userRepository)
        {
            if (!IsUserInBusinessContext(userName))
                return null;
            if (IsAuthenticatedUser(userName))
                return userName;
            var user = LookupBusinessAdminUser(userRepository);
            return user.Email;
        }

        private bool IsUserInBusinessContext(string userName)
        {
            return BusinessId.HasValue && userName != "";
        }

        private bool IsAuthenticatedUser(string userName)
        {
            return userName != Constants.ANONYMOUS_USER;
        }

        private User LookupBusinessAdminUser(IUserRepository userRepository)
        {
            var user = userRepository.GetByBusinessId(BusinessId.Value);
            if (user.IsNotFound())
                throw new InvalidOperationException("User not associated with a business.");
            return user;
        }
    }
}
