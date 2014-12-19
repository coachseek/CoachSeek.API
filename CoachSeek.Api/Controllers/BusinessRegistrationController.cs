using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class BusinessRegistrationController : BaseController
    {
        public IUserAddUseCase UserAddUseCase { get; set; }
        public IBusinessNewRegistrationUseCase BusinessNewRegistrationUseCase { get; set; }
        public IUserAssociateWithBusinessUseCase UserAssociateWithBusinessUseCase { get; set; }

        public BusinessRegistrationController()
        { }

        public BusinessRegistrationController(IUserAddUseCase userAddUseCase,
                                              IBusinessNewRegistrationUseCase businessNewRegistrationUseCase,
                                              IUserAssociateWithBusinessUseCase userAssociateWithBusinessUseCase)
        {
            UserAddUseCase = userAddUseCase;
            BusinessNewRegistrationUseCase = businessNewRegistrationUseCase;
            UserAssociateWithBusinessUseCase = userAssociateWithBusinessUseCase;
        }

        // POST: api/BusinessRegistration
        [AllowAnonymous]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            var userAddCommand = UserAddCommandConverter.Convert(registration.Registrant);
            var userAddResponse = UserAddUseCase.AddUser(userAddCommand);
            if (!userAddResponse.IsSuccessful)
                return CreateWebResponse(userAddResponse);

            var businessAddCommand = BusinessAddCommandConverter.Convert(registration);
            var businessAddResponse = BusinessNewRegistrationUseCase.RegisterNewBusiness(businessAddCommand);
            if (!businessAddResponse.IsSuccessful)
                return CreateWebResponse(businessAddResponse);

            // var associate user to business use case.
            // Link a user to a business in a capacity/role.
            var userBusinessCommand = UserAssociateWithBusinessCommandBuilder.BuildCommand(userAddResponse.Data, businessAddResponse.Data);
            var userResponse = UserAssociateWithBusinessUseCase.AssociateUserWithBusiness(userBusinessCommand);

            // TODO: We will have to include the business info. 
            return CreateWebResponse(userResponse);
        }
    }
}
