using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        private readonly PaymentOptions _payment;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Domain { get; private set; }
        public string Currency { get { return _payment.CurrencyCode; } }
        public bool IsOnlinePaymentEnabled { get { return _payment.IsOnlinePaymentEnabled; } }
        public bool? ForceOnlinePayment { get { return _payment.ForceOnlinePayment; } }
        public string PaymentProvider { get { return _payment.PaymentProvider; } }
        public string MerchantAccountIdentifier { get { return _payment.MerchantAccountIdentifier; } }


        public Business(BusinessAddCommand command, 
                        IBusinessDomainBuilder domainBuilder, 
                        ISupportedCurrencyRepository supportedCurrencyRepository) 
        {
            Id = Guid.NewGuid();
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
            _payment = new PaymentOptions(command, supportedCurrencyRepository);
        }

        public Business(Guid businessId, 
                        BusinessUpdateCommand command, 
                        ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            Id = businessId;
            Name = command.Name.Trim();
            _payment = new PaymentOptions(command, supportedCurrencyRepository);
        }

        public Business()
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
            _payment = new PaymentOptions(currency, 
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