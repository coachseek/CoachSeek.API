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

        public BusinessRegistrationResponse RegisterNewBusiness(BusinessRegistrationCommand registrationCommand)
        {
            if (registrationCommand == null)
                return new NoBusinessRegistrationDataResponse();

            try
            {
                var newBusiness = new NewBusiness(registrationCommand, BusinessDomainBuilder);
                newBusiness.Register(BusinessRepository);
                SendRegistrationEmail(newBusiness.ToData());
                return new BusinessRegistrationResponse(newBusiness.ToData());
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

        private BusinessRegistrationResponse HandleBusinessRegistrationException(Exception ex)
        {
            if (ex is DuplicateBusinessAdmin)
                return HandleDuplicateBusinessAdmin();

            return null;
        }

        private BusinessRegistrationResponse HandleDuplicateBusinessAdmin()
        {
            return new DuplicateBusinessAdminBusinessRegistrationResponse();
        }
    }
}