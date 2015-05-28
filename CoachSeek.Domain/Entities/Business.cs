using AutoMapper;
using CoachSeek.Data.Model;
using System;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get; protected set; }
        public Currency Currency { get; protected set; }


        public Business(BusinessAddCommand command, IBusinessDomainBuilder domainBuilder, ISupportedCurrencyRepository supportedCurrencyRepository) 
            : this()
        {
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
            Currency = new Currency(command.Currency, supportedCurrencyRepository);
        }

        public Business(Guid businessId, BusinessUpdateCommand command, ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            Id = businessId;
            Name = command.Name.Trim();
            Currency = new Currency(command.Currency, supportedCurrencyRepository);
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
    }
}