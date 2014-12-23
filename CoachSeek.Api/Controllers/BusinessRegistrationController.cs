using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Services.Contracts.Email;

namespace CoachSeek.Api.Controllers
{
    public class BusinessRegistrationController : BaseController
    {
        public IUserAddUseCase UserAddUseCase { get; set; }
        public IBusinessAddUseCase BusinessAddUseCase { get; set; }
        public IUserAssociateWithBusinessUseCase UserAssociateWithBusinessUseCase { get; set; }
        public IBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }


        public BusinessRegistrationController()
        { }

        public BusinessRegistrationController(IUserAddUseCase userAddUseCase,
                                              IBusinessAddUseCase businessAddUseCase,
                                              IUserAssociateWithBusinessUseCase userAssociateWithBusinessUseCase,
                                              IBusinessRegistrationEmailer businessRegistrationEmailer)
        {
            UserAddUseCase = userAddUseCase;
            BusinessAddUseCase = businessAddUseCase;
            UserAssociateWithBusinessUseCase = userAssociateWithBusinessUseCase;
            BusinessRegistrationEmailer = businessRegistrationEmailer;
        }


        // POST: api/BusinessRegistration
        [AllowAnonymous]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            var userAddCommand = UserAddCommandConverter.Convert(registration.Admin);
            var userAddResponse = UserAddUseCase.AddUser(userAddCommand);
            if (!userAddResponse.IsSuccessful)
                return CreateWebErrorResponse(userAddResponse);
            var userData = userAddResponse.Data;

            var businessAddCommand = BusinessAddCommandConverter.Convert(registration.Business);
            var businessAddResponse = BusinessAddUseCase.AddBusiness(businessAddCommand);
            if (!businessAddResponse.IsSuccessful)
                return CreateWebErrorResponse(businessAddResponse);
            var businessData = businessAddResponse.Data;

            // var associate user to business use case.
            // Link a user to a business in a capacity/role.
            var userBusinessCommand = UserAssociateWithBusinessCommandBuilder.BuildCommand(userData, businessData);
            var userUpdateResponse = UserAssociateWithBusinessUseCase.AssociateUserWithBusiness(userBusinessCommand);
            if (!userUpdateResponse.IsSuccessful)
                return CreateWebErrorResponse(userUpdateResponse);

            var registrationData = new RegistrationData(userData, businessData);
            SendRegistrationEmail(registrationData);

            // TODO: We will have to include the business info.
            return CreateWebSuccessResponse(new Response<RegistrationData>(registrationData));
        }


        private void SendRegistrationEmail(RegistrationData registration)
        {
            BusinessRegistrationEmailer.SendEmail(registration);
        }
    }
}
