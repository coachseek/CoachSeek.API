using CoachSeek.WebUI.Contracts.Builders;
using CoachSeek.WebUI.Contracts.Email;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.UseCases;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class BusinessRegistrationController : ApiController
    {
        private IBusinessRepository BusinessRepository { get; set; }
        private IBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }
        private IBusinessDomainBuilder BusinessDomainBuilder { get; set; }

        public BusinessRegistrationController(IBusinessRepository businessRepository,
                                              IBusinessDomainBuilder businessDomainBuilder,
                                              IBusinessRegistrationEmailer businessRegistrationEmailer)
        {
            BusinessRepository = businessRepository;
            BusinessRegistrationEmailer = businessRegistrationEmailer;
            BusinessDomainBuilder = businessDomainBuilder;
        }

        // POST: api/BusinessRegistration
        public HttpResponseMessage Post([FromBody]BusinessRegistrationRequest businessRegistration)
        {
            var useCase = new BusinessNewRegistrationUseCase(BusinessRepository, BusinessDomainBuilder, BusinessRegistrationEmailer);
            var response = useCase.RegisterNewBusiness(businessRegistration);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}
