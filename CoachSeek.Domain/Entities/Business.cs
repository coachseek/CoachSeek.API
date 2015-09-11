using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        protected PaymentOptions Payment { get; set; }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get; protected set; }
        public string Sport { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public string Currency { get { return Payment.CurrencyCode; } }
        public bool IsOnlinePaymentEnabled { get { return Payment.IsOnlinePaymentEnabled; } }
        public bool? ForceOnlinePayment { get { return Payment.ForceOnlinePayment; } }
        public string PaymentProvider { get { return Payment.PaymentProvider; } }
        public string MerchantAccountIdentifier { get { return Payment.MerchantAccountIdentifier; } }


        public Business(BusinessData existingBusiness, 
                        BusinessUpdateCommand command, 
                        ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            Id = existingBusiness.Id;
            Name = command.Name.Trim();
            Domain = existingBusiness.Domain;
            Sport = existingBusiness.Sport;
            Payment = new PaymentOptions(command, supportedCurrencyRepository);
        }

        protected Business()
        {
            Id = Guid.NewGuid();
        }

        // Minimal Unit testing constructor.
        public Business(Guid id)
        {
            Id = id;
        }

        public Business(Guid id, 
            string name,
            string domain,
            string sport,
            string currency,
            bool isOnlinePaymentEnabled = false,
            bool forceOnlinePayment = false,
            string paymentProvider = null,
            string merchantAccountIdentifier = null)
        {
            // Testing constructor

            Id = id;
            Name = name;
            Domain = domain;
            Sport = sport;
            Payment = new PaymentOptions(currency, 
                                         isOnlinePaymentEnabled, 
                                         forceOnlinePayment,
                                         paymentProvider,
                                         merchantAccountIdentifier);
        }


        public BusinessData ToData()
        {
            var businessData = Mapper.Map<Business, BusinessData>(this);

            businessData.Payment.Currency = Currency;
            businessData.Payment.IsOnlinePaymentEnabled = IsOnlinePaymentEnabled;
            businessData.Payment.ForceOnlinePayment = ForceOnlinePayment.GetValueOrDefault();
            businessData.Payment.PaymentProvider = PaymentProvider;
            businessData.Payment.MerchantAccountIdentifier = MerchantAccountIdentifier;

            return businessData;
        }
    }
}