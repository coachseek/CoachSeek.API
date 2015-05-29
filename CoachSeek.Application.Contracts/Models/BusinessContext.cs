using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class BusinessContext
    {
        public BusinessDetails Business { get; private set; }
        public CurrencyDetails Currency { get; private set; }
        public string AdminEmail { get; private set; }
        public IBusinessRepository BusinessRepository { get; private set; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { get; private set; }


        public BusinessContext(BusinessDetails business, 
                               CurrencyDetails currency, 
                               string userName,
                               IBusinessRepository businessRepository,
                               ISupportedCurrencyRepository supportedCurrencyRepository,
                               IUserRepository userRepository)
        {
            Business = business;
            Currency = currency;
            BusinessRepository = businessRepository;
            SupportedCurrencyRepository = supportedCurrencyRepository;
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
            if (Business == null)
                return false;
            return Business.Id != Guid.Empty && userName != "";
        }

        private bool IsAuthenticatedUser(string userName)
        {
            return userName != Constants.ANONYMOUS_USER;
        }

        private User LookupBusinessAdminUser(IUserRepository userRepository)
        {
            var user = userRepository.GetByBusinessId(Business.Id);
            if (user.IsNotFound())
                throw new InvalidOperationException("User not associated with a business.");
            return user;
        }
    }
}
