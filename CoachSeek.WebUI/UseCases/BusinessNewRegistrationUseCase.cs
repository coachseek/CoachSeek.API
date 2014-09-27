using CoachSeek.WebUI.Contracts.Builders;
using CoachSeek.WebUI.Contracts.Email;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Extensions;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.Responses;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.UseCases
{
    public class BusinessNewRegistrationUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }
        private IBusinessAdminRepository BusinessAdminRepository { get; set; }
        private IBusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private IBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }

        public BusinessNewRegistrationUseCase(IBusinessRepository businessRepository, 
                                              IBusinessAdminRepository businessAdminRepository,
                                              IBusinessDomainBuilder businessDomainBuilder,
                                              IBusinessRegistrationEmailer businessRegistrationEmailer)
        {
            BusinessRepository = businessRepository;
            BusinessAdminRepository = businessAdminRepository;
            BusinessDomainBuilder = businessDomainBuilder;
            BusinessRegistrationEmailer = businessRegistrationEmailer;
        }

        public BusinessRegistrationResponse RegisterNewBusiness(BusinessRegistrationRequest registration)
        {
            if (registration == null)
                return new NoBusinessRegistrationDataResponse();

            try
            {
                Validate(registration);
                var newbusiness = Register(registration);
                SendRegistrationEmail(newbusiness);
                return new BusinessRegistrationResponse(newbusiness);
            }
            catch (ValidationException valEx)
            {
                return new BusinessRegistrationResponse(valEx);
            }
        }

        private void Validate(BusinessRegistrationRequest registration)
        {
            var admin = BusinessAdminRepository.GetByEmail(registration.Registrant.Email);
            if (admin.IsExisting())
                ThrowBusinessAdminDuplicateEmailValiationException();       
        }

        private void ThrowBusinessAdminDuplicateEmailValiationException()
        {
            throw new ValidationException((int)ErrorCodes.ErrorBusinessAdminDuplicateEmail, 
                                          Resources.ErrorBusinessAdminDuplicateEmail,
                                          FormField.Email);
        }

        private Business Register(BusinessRegistrationRequest registration)
        {
            var business = RegisterBusiness(registration);
            business.Admin = RegisterBusinessAdmin(registration, business);
            return business;
        }

        private Business RegisterBusiness(BusinessRegistrationRequest registration)
        {
            var newBusiness = BusinessConverter.Convert(registration);
            newBusiness.Domain = BusinessDomainBuilder.BuildDomain(newBusiness.Name);
            return BusinessRepository.Add(newBusiness);
        }

        private BusinessAdmin RegisterBusinessAdmin(BusinessRegistrationRequest registration, Business business)
        {
            var admin = BusinessAdminConverter.Convert(registration.Registrant, business.Id);
            return BusinessAdminRepository.Add(admin);
        }

        private void SendRegistrationEmail(Business newbusiness)
        {
            BusinessRegistrationEmailer.SendEmail(newbusiness);
        }
    }
}