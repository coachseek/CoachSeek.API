using System;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class NewBusiness : Business
    {
        public DateTime CreatedOn { get; set; }
        public bool IsTesting { get; set; }

        public NewBusiness(BusinessRegistrationCommand registration, 
                           IBusinessDomainBuilder domainBuilder, 
                           ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var command = registration.Business;
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
            Payment = new PaymentOptions(command, supportedCurrencyRepository);
            CreatedOn = DateTime.Now; // On Windows Azure this is UTC/GMT time.
            IsTesting = DetermineIsTesting(registration.Admin.Email);
        }

        public NewBusiness(Guid id, 
                           string name, 
                           string domain,
                           string currency,
                           bool isOnlinePaymentEnabled = false,
                           bool forceOnlinePayment = false,
                           string paymentProvider = null,
                           string merchantAccountIdentifier = null,
                           DateTime createdOn = new DateTime(),
                           bool isTesting = true)
            : base(id, 
                   name, 
                   domain, 
                   currency, 
                   isOnlinePaymentEnabled, 
                   forceOnlinePayment, 
                   paymentProvider, 
                   merchantAccountIdentifier)
        {
            // Testing constructor

            CreatedOn = DateTime.Now;
            IsTesting = isTesting;
        }


        private bool DetermineIsTesting(string email)
        {
            return email.ToLower().EndsWith("@coachseek.com");
        }
    }
}
