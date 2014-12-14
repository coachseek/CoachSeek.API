using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class BusinessRegistrationController : BaseController
    {
        public IUserAddUseCase UserAddUseCase { get; set; }
        public IBusinessNewRegistrationUseCase BusinessNewRegistrationUseCase { get; set; }

        public BusinessRegistrationController()
        { }

        public BusinessRegistrationController(IUserAddUseCase userAddUseCase,
                                              IBusinessNewRegistrationUseCase businessNewRegistrationUseCase)
        {
            UserAddUseCase = userAddUseCase;
            BusinessNewRegistrationUseCase = businessNewRegistrationUseCase;
        }

        // POST: api/BusinessRegistration
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            // TODO: Make this transactional

            //var userCommand = UserAddCommandConverter.Convert(registration.Registrant);
            //var userAddResponse = UserAddUseCase.AddUser(userCommand);
            //if (!userAddResponse.IsSuccessful)
            //    return CreateWebResponse(userAddResponse);

            var command = BusinessAddCommandConverter.Convert(registration);
            var response = BusinessNewRegistrationUseCase.RegisterNewBusiness(command);
            return CreateWebResponse(response);

            // var associate user to business use case.
            // Link a user to a business in a capacity/role.
        }
    }
}
