﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using CoachSeek.Services.Contracts.Builders;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BusinessAddUseCase : IBusinessAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }
        private IBusinessDomainBuilder BusinessDomainBuilder { get; set; }

        public BusinessAddUseCase(IBusinessRepository businessRepository, 
                                  IBusinessDomainBuilder businessDomainBuilder)
        {
            BusinessRepository = businessRepository;
            BusinessDomainBuilder = businessDomainBuilder;
        }

        public Response AddBusiness(BusinessAddCommand command)
        {
            if (command == null)
                return new MissingBusinessRegistrationDataErrorResponse();

            try
            {
                var newBusiness = new NewBusiness(command, BusinessDomainBuilder);
                var business = newBusiness.Register(BusinessRepository);
                return new Response(business);
            }
            catch (Exception ex)
            {
                return HandleBusinessRegistrationException(ex);
            }
        }

        private Response HandleBusinessRegistrationException(Exception ex)
        {
            if (ex is DuplicateBusinessAdmin)
                return new DuplicateBusinessAdminErrorResponse();

            return null;
        }
    }
}