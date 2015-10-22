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
                               ISupportedCurrencyRepository supportedCurrencyRepository,
                               IUserRepository userRepository)
        {
            Business = business;
            BusinessRepository = businessRepository;
            SupportedCurrencyRepository = supportedCurrencyRepository;
            AdminEmail = LookupBusinessAdminUser(userRepository).Email;
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
