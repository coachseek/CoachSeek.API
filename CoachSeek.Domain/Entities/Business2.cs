using AutoMapper;
using CoachSeek.Data.Model;
using System;
using CoachSeek.Domain.Commands;
using IBusinessDomainBuilder = CoachSeek.Domain.Contracts.IBusinessDomainBuilder;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get; protected set; }


        public Business(BusinessAddCommand command, IBusinessDomainBuilder domainBuilder) 
            : this()
        {
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
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