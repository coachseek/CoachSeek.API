using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class BusinessContext
    {
        public Business Business { get; private set; }
        public string AdminEmail { get; private set; }
        public IBusinessRepository BusinessRepository { get; private set; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { get; private set; }


        public BusinessContext(Business business,
                               IBusinessRepository businessRepository)
        {
            Business = business;
            BusinessRepository = businessRepository;
        }

        public BusinessContext(Business business, 
                               IBusinessRepository businessRepository,
                               IUserRepository userRepository,
                               ISupportedCurrencyRepository supportedCurrencyRepository = null)
        {
            Business = business;
            BusinessRepository = businessRepository;
            AdminEmail = LookupBusinessAdminUser(userRepository).Email;
            SupportedCurrencyRepository = supportedCurrencyRepository;
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
