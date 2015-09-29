﻿using System;
using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Repositories;
using IBusinessDomainBuilder = CoachSeek.Domain.Contracts.IBusinessDomainBuilder;

namespace CoachSeek.Domain.Services
{
    public class BusinessDomainBuilder : IBusinessDomainBuilder
    {
        private IReservedDomainRepository ReservedDomainRepository { get; set; }
        public IBusinessRepository BusinessRepository { get; set; }

        public BusinessDomainBuilder(IReservedDomainRepository reservedDomainRepository)
        {
            ReservedDomainRepository = reservedDomainRepository;
        }

        public async Task<string> BuildDomainAsync(string businessName)
        {
            if (string.IsNullOrWhiteSpace(businessName))
                throw new ArgumentNullException("businessName");
            var domain = RemoveNonAlphaNumericCharacters(businessName).ToLowerInvariant();
            while (await IsUnavailableDomain(domain))
                domain = AlterDomain(domain);
            return domain;
        }

        private async Task<bool> IsUnavailableDomain(string domain)
        {
            var isReservedDomain = ReservedDomainRepository.Contains(domain);
            if (isReservedDomain)
                return true;
            return (await BusinessRepository.GetBusinessAsync(domain)).IsFound();
        }

        private static string AlterDomain(string domain)
        {
            var lastChar = domain.GetLastCharacter();
            var lastDigit = lastChar.Parse<int>();
            if (lastDigit == 0)
                return string.Format("{0}1", domain);

            var domainWithoutLastCharacter = domain.Substring(0, domain.Length - 1);
            return string.Format("{0}{1}", domainWithoutLastCharacter, ++lastDigit);
        }

        private static string RemoveNonAlphaNumericCharacters(string businessName)
        {
            var array = businessName.ToCharArray();
            array = Array.FindAll(array, (c => (char.IsLetterOrDigit(c))));
            businessName = new string(array);
            return businessName;
        }
    }
}