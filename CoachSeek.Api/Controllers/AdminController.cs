using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Application.Contracts.UseCases.Admin;

namespace CoachSeek.Api.Controllers
{
    public class AdminController : BaseController
    {
        public IEmailUnsubscribeUseCase EmailUnsubscribeUseCase { get; set; }

        public AdminController()
        { }

        public AdminController(IEmailUnsubscribeUseCase emailUnsubscribeUseCase)
        {
            EmailUnsubscribeUseCase = emailUnsubscribeUseCase;
        }


        // GET: Admin/Unsubscribe/olaf@coachseek.com
        [Route("Admin/Email/Unsubscribe/{email}")]
        [BasicAuthentication]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage Unsubscribe(string email)
        {
            EmailUnsubscribeUseCase.Initialise(Context);
            EmailUnsubscribeUseCase.Unsubscribe(email);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET: Admin/IsUnsubscribed/olaf@coachseek.com
        [Route("Admin/Email/IsUnsubscribed/{email}")]
        [BasicAuthentication]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage IsUnsubscribed(string email)
        {
            //EmailSetToIsUnsubscribedUseCase.Initialise(Context);
            //var response = EmailSetToIsUnsubscribedUseCase.Unsubscribe(email);
            //return CreateGetWebResponse(response);
            return null;
        }
    }
}
