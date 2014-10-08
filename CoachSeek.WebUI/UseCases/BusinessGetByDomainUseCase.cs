﻿using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class BusinessGetByDomainUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public BusinessGetByDomainUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public BusinessGetResponse GetByDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return new NoDomainBusinessGetResponse();

            var business = BusinessRepository.GetByDomain(domain);
            return new BusinessGetResponse(business);
        }
    }
}