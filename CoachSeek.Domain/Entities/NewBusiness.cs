using System;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class NewBusiness : Business
    {
        private const int FREE_TRIAL_DURATION_IN_DAYS = 30;

        public DateTime CreatedOn { get; set; }
        public bool IsTesting { get; set; }

        public NewBusiness(BusinessRegistrationCommand registration, 
                           IBusinessDomainBuilder domainBuilder, 
                           ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var command = registration.Business;
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
            Sport = command.Sport == null ? null : command.Sport.Trim();
            Payment = new PaymentOptions(command, supportedCurrencyRepository);
            IsTesting = DetermineIsTesting(registration.Admin.Email);
            SetDates();
            SetSubsciption();
        }

        public NewBusiness(Guid id, 
                           string name, 
                           string domain,
                           string currency,
                           string sport = null,
                           bool isOnlinePaymentEnabled = false,
                           bool forceOnlinePayment = false,
                           string paymentProvider = null,
                           string merchantAccountIdentifier = null,
                           DateTime? createdOn = null,
                           bool isTesting = true)
            : base(id, 
                   name, 
                   domain, 
                   sport,
                   currency, 
                   isOnlinePaymentEnabled, 
                   forceOnlinePayment, 
                   paymentProvider, 
                   merchantAccountIdentifier)
        {
            // Testing constructor

            IsTesting = isTesting;
            SetDates(createdOn);
            SetSubsciption();
        }


        private bool DetermineIsTesting(string email)
        {
            return email.ToLower().EndsWith("@coachseek.com");
        }

        private void SetDates(DateTime? createdOn = null)
        {
            CreatedOn = createdOn ?? DateTime.UtcNow;
            AuthorisedUntil = CreatedOn.AddDays(FREE_TRIAL_DURATION_IN_DAYS);
        }

        private void SetSubsciption()
        {
            Subscription = CoachSeek.Domain.Entities.Subscription.Create(Constants.SUBSCRIPTION_TRIAL);
        }
    }
}
