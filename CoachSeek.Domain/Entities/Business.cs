using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Factories;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get; protected set; }
        public Currency Currency { get; protected set; }
        public PaymentProviderBase Payment { get; protected set; }


        public Business(BusinessAddCommand command, IBusinessDomainBuilder domainBuilder, ISupportedCurrencyRepository supportedCurrencyRepository) 
            : this()
        {
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
            Currency = new Currency(command.Currency, supportedCurrencyRepository);
            Payment = PaymentProviderFactory.CreateDefaultPaymentProvider();
        }

        public Business(Guid businessId, BusinessUpdateCommand command, ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var errors = new ValidationException();

            Id = businessId;
            Name = command.Name.Trim();
            SetCurrency(command, supportedCurrencyRepository, errors);
            SetPaymentProvider(command, errors);

            errors.ThrowIfErrors();
        }

        public Business()
        {
            Id = Guid.NewGuid();
        }

        // Minimal Unit testing constructor.
        public Business(Guid id)
            : this()
        {
            Id = id;
        }

        public Business(Guid id, 
            string name, 
            string domain)
        {
            Id = id;
            Name = name;
            Domain = domain;
        }


        public BusinessData ToData()
        {
            return Mapper.Map<Business, BusinessData>(this);
        }


        private void SetCurrency(BusinessUpdateCommand command, ISupportedCurrencyRepository supportedCurrencyRepository, ValidationException errors)
        {
            try
            {
                Currency = new Currency(command.Currency, supportedCurrencyRepository);
            }
            catch (CurrencyNotSupported)
            {
                errors.Add("This currency is not supported.", "business.currency");
            }
        }

        private void SetPaymentProvider(BusinessUpdateCommand command, ValidationException errors)
        {
            try
            {
                Payment = PaymentProviderFactory.CreatePaymentProvider(command.PaymentProvider, command.MerchantAccountIdentifier);
            }
            catch (Exception ex)
            {
                if (ex is PaymentProviderNotSupported)
                    errors.Add("This payment provider is not supported.", "business.paymentProvider");
                if (ex is MissingMerchantAccountIdentifier)
                    errors.Add("Missing merchant account identifier.", "business.merchantAccountIdentifier");
                if (ex is InvalidMerchantAccountIdentifierFormat)
                    errors.Add("Invalid merchant account identifier format.", "business.merchantAccountIdentifier");
            }
        }
    }
}