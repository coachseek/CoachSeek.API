using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using CoachSeek.Services.Contracts.Builders;
using CoachSeek.Services.Contracts.Email;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BusinessNewRegistrationUseCase : IBusinessNewRegistrationUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }
        private IBusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private IBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }

        public BusinessNewRegistrationUseCase(IBusinessRepository businessRepository, 
                                              IBusinessDomainBuilder businessDomainBuilder,
                                              IBusinessRegistrationEmailer businessRegistrationEmailer)
        {
            BusinessRepository = businessRepository;
            BusinessDomainBuilder = businessDomainBuilder;
            BusinessRegistrationEmailer = businessRegistrationEmailer;
        }

        public Response<BusinessData> RegisterNewBusiness(BusinessRegistrationCommand registrationCommand)
        {
            if (registrationCommand == null)
                return new NoBusinessRegistrationDataResponse();

            try
            {
                var newBusiness = new NewBusiness(registrationCommand, BusinessDomainBuilder);
                var business = newBusiness.Register(BusinessRepository);
                SendRegistrationEmail(business);
                return new Response<BusinessData>(business);
            }
            catch (Exception ex)
            {
                return HandleBusinessRegistrationException(ex);
            }
        }

        private void SendRegistrationEmail(BusinessData newbusiness)
        {
            BusinessRegistrationEmailer.SendEmail(newbusiness);
        }

        private Response<BusinessData> HandleBusinessRegistrationException(Exception ex)
        {
            if (ex is DuplicateBusinessAdmin)
                return HandleDuplicateBusinessAdmin();

            return null;
        }

        private Response<BusinessData> HandleDuplicateBusinessAdmin()
        {
            return new DuplicateBusinessAdminBusinessRegistrationResponse();
        }
    }
}