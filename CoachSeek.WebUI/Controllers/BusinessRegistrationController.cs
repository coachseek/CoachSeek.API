using CoachSeek.WebUI.Builders;
using CoachSeek.WebUI.Email;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Controllers
{
    public class BusinessRegistrationController : ApiController
    {
        private static List<Business> Businesses { get; set; }

        static BusinessRegistrationController()
        {
            Businesses = new List<Business>();
        }


        // POST: api/BusinessRegistration
        public HttpResponseMessage Post([FromBody]BusinessRegistrationRequest businessRegistration)
        {
            var reservedDomainRepository = new HardCodedReservedDomainRepository();
            var businessRepository = new InMemoryBusinessRepository();
            var businessAdminRepository = new InMemoryBusinessAdminRepository();
            var domainBuilder = new BusinessDomainBuilder(reservedDomainRepository, businessRepository);
            var emailer = new StubBusinessRegistrationEmailer();

            var useCase = new BusinessNewRegistrationUseCase(businessRepository, businessAdminRepository, domainBuilder, emailer);

            var response = useCase.RegisterNewBusiness(businessRegistration);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK, response.Business);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors[0]);
        }
    }
}
