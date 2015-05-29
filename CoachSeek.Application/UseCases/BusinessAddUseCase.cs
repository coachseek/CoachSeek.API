﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BusinessAddUseCase : BaseUseCase, IBusinessAddUseCase
    {
        private IBusinessDomainBuilder BusinessDomainBuilder { get; set; }

        public BusinessAddUseCase(IBusinessDomainBuilder businessDomainBuilder)
        {
            BusinessDomainBuilder = businessDomainBuilder;
        }

        public Response AddBusiness(BusinessAddCommand command)
        {
            if (command == null)
                return new MissingBusinessRegistrationDataErrorResponse();

            try
            {
                BusinessDomainBuilder.BusinessRepository = BusinessRepository;
                var newBusiness = new Business(command, BusinessDomainBuilder, SupportedCurrencyRepository);
                var data = BusinessRepository.AddBusiness(newBusiness);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is CurrencyNotSupported)
                    return new CurrencyNotSupportedErrorResponse();
                throw;
            }
        }
    }
}