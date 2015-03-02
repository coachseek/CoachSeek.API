using CoachSeek.Application.Contracts.Models;
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
                var newBusiness = new Business2(command, BusinessDomainBuilder);
                var data = BusinessRepository.AddBusiness(newBusiness);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is DuplicateBusinessAdmin)
                    return new DuplicateBusinessAdminErrorResponse();
                throw;
            }
        }

        //private Response HandleBusinessRegistrationException(Exception ex)
        //{
        //    if (ex is DuplicateBusinessAdmin)
        //        return new DuplicateBusinessAdminErrorResponse();

        //    return null;
        //}
    }
}