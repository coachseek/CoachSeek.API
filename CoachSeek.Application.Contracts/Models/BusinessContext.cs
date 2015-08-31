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
                               IBusinessRepository businessRepository,
                               ISupportedCurrencyRepository supportedCurrencyRepository,
                               IUserRepository userRepository)
        {
            Business = business;
            Currency = currency;
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
