using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Application.Contracts.UseCases.Admin;

namespace CoachSeek.Api.Controllers
{
    public class AdminController : BaseController
    {
        public IEmailSetToIsUnsubscribedUseCase EmailSetToIsUnsubscribedUseCase { get; set; }

        public AdminController()
        { }

        public AdminController(IEmailSetToIsUnsubscribedUseCase emailSetToIsUnsubscribedUseCase)
        {
            EmailSetToIsUnsubscribedUseCase = emailSetToIsUnsubscribedUseCase;
        }


        // GET: Admin/Unsubscribe/olaf@coachseek.com
        [Route("Admin/Email/Unsubscribe/{email}")]
        [BasicAuthentication]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage Unsubscribe(string email)
        {
            //EmailSetToIsUnsubscribedUseCase.Initialise(Context);
            //var response = EmailSetToIsUnsubscribedUseCase.Unsubscribe(email);
            //return CreateGetWebResponse(response);
            return null;
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
