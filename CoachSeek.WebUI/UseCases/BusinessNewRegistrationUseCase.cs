using CoachSeek.WebUI.Contracts.Builders;
using CoachSeek.WebUI.Contracts.Email;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Factories;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class BusinessNewRegistrationUseCase
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

        public BusinessRegistrationResponse RegisterNewBusiness(BusinessRegistrationRequest registration)
        {
            if (registration == null)
                return new NoBusinessRegistrationDataResponse();

            try
            {
                var newBusiness = BusinessFactory.Create(registration, BusinessDomainBuilder);
                newBusiness.Register(BusinessRepository);
                SendRegistrationEmail(newBusiness);
                return new BusinessRegistrationResponse(newBusiness);
            }
            catch (ValidationException valEx)
            {
                return new BusinessRegistrationResponse(valEx);
            }
        }

        private void SendRegistrationEmail(Business newbusiness)
        {
            BusinessRegistrationEmailer.SendEmail(newbusiness);
        }
    }
}