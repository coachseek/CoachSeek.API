using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities.Subscriptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        protected Subdomain Subdomain { get; set; }
        protected PaymentOptions Payment { get; set; }
        protected Subscription Subscription { get; set; }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get { return Subdomain.Domain; } }
        public string Sport { get; protected set; }
        public string SubscriptionPlan { get { return Subscription.Plan; } }
        public DateTime AuthorisedUntil { get; protected set; }
        public string CurrencyCode { get { return Payment.CurrencyCode; } }
        public string CurrencySymbol { get { return Payment.CurrencySymbol; } }
        public bool IsOnlinePaymentEnabled { get { return Payment.IsOnlinePaymentEnabled; } }
        public bool? ForceOnlinePayment { get { return Payment.ForceOnlinePayment; } }
        public string PaymentProvider { get { return Payment.PaymentProvider; } }
        public string MerchantAccountIdentifier { get { return Payment.MerchantAccountIdentifier; } }
        public bool UseProRataPricing { get { return Payment.UseProRataPricing; } }

        public Business(BusinessData existingBusiness, 
                        BusinessUpdateCommand command, 
                        ISupportedCurrencyRepository supportedCurrencyRepository) 
            : this (existingBusiness)
        {
            Name = command.Name.Trim();
            Subdomain = new Subdomain(command.Domain);
            Sport = command.Sport;
            Payment = new PaymentOptions(command, supportedCurrencyRepository);
        }

        public Business(BusinessData business, ISupportedCurrencyRepository supportedCurrencyRepository)
            : this(business)
        {
            Payment = new PaymentOptions(business.Payment, supportedCurrencyRepository);
        }

        private Business(BusinessData business)
        {
            Id = business.Id;
            Name = business.Name;
            Subdomain = new Subdomain(business.Domain);
            Sport = business.Sport;
            AuthorisedUntil = business.AuthorisedUntil;
            Subscription = Subscription.Create(business.SubscriptionPlan);
        }

        protected Business()
        {
            Id = Guid.NewGuid();
        }

        public Business(Guid id)
        {
            Id = id;
        }

        public Business(Guid id,
            string name,
            string domain,
            string currencyCode,
            string currencySymbol,
            string sport,
            bool isOnlinePaymentEnabled = false,
            bool forceOnlinePayment = false,
            string paymentProvider = null,
            string merchantAccountIdentifier = null)
        {
            // Testing constructor

            Id = id;
            Name = name;
            Subdomain = new Subdomain(domain);
            Sport = sport;
            Payment = new PaymentOptions(currencyCode,
                                         currencySymbol,
                                         isOnlinePaymentEnabled,
                                         forceOnlinePayment,
                                         paymentProvider,
                                         merchantAccountIdentifier);
        }

        public Business(Guid id, 
            string name,
            string domain,
            string currencyCode,
            string currencySymbol,
            string sport,
            DateTime authorisedUntil,
            string subscription,
            bool isOnlinePaymentEnabled = false,
            bool forceOnlinePayment = false,
            string paymentProvider = null,
            string merchantAccountIdentifier = null)
        {
            // Testing constructor

            Id = id;
            Name = name;
            Subdomain = new Subdomain(domain);
            Sport = sport;
            AuthorisedUntil = authorisedUntil;
            Subscription = Subscription.Create(subscription);
            Payment = new PaymentOptions(currencyCode,
                                         currencySymbol,
                                         isOnlinePaymentEnabled, 
                                         forceOnlinePayment,
                                         paymentProvider,
                                         merchantAccountIdentifier);
        }


        public BusinessData ToData()
        {
            var businessData = Mapper.Map<Business, BusinessData>(this);

            businessData.Payment.Currency = CurrencyCode;
            businessData.Payment.IsOnlinePaymentEnabled = IsOnlinePaymentEnabled;
            businessData.Payment.ForceOnlinePayment = ForceOnlinePayment.GetValueOrDefault();
            businessData.Payment.PaymentProvider = PaymentProvider;
            businessData.Payment.MerchantAccountIdentifier = MerchantAccountIdentifier;
            businessData.Payment.UseProRataPricing = UseProRataPricing;

            return businessData;
        }
    }
}