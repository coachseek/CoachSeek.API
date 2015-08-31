using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Admin;

namespace CoachSeek.Api.Controllers
{
    public class AdminController : BaseController
    {
        public IEmailUnsubscribeUseCase EmailUnsubscribeUseCase { get; set; }
        public IEmailIsUnsubscribedUseCase EmailIsUnsubscribedUseCase { get; set; }
        public IUserGetByEmailUserCase UserGetByEmailUserCase { get; set; }

        public AdminController()
        { }

        public AdminController(IEmailUnsubscribeUseCase emailUnsubscribeUseCase,
                               IEmailIsUnsubscribedUseCase emailIsUnsubscribedUseCase,
                               IUserGetByEmailUserCase userGetByEmailUserCase)
        {
            EmailUnsubscribeUseCase = emailUnsubscribeUseCase;
            EmailIsUnsubscribedUseCase = emailIsUnsubscribedUseCase;
            UserGetByEmailUserCase = userGetByEmailUserCase;
        }


        // GET: Admin/Email/Unsubscribe?email=olaf@coachseek.com
        [Route("Admin/Email/Unsubscribe")]
        [BasicAdminAuthentication]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Unsubscribe(string email)
        {
            EmailUnsubscribeUseCase.Initialise(Context);
            var response = EmailUnsubscribeUseCase.Unsubscribe(email);
            if (response.IsSuccessful)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest, response.Errors);
        }

        // GET: Admin/Email/IsUnsubscribed?email=olaf@coachseek.com
        [Route("Admin/Email/IsUnsubscribed")]
        [BasicAdminAuthentication]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage IsUnsubscribed(string email)
        {
            EmailIsUnsubscribedUseCase.Initialise(Context);
            var isUnsubscribed = EmailIsUnsubscribedUseCase.IsUnsubscribed(email);
            if (!isUnsubscribed)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.Found);
        }

        // GET: Admin/Users/olaf@coachseek.com
        [Route("Admin/Users/{email}")]
        [BasicAdminAuthentication]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetUser(string email)
        {
            UserGetByEmailUserCase.Initialise(Context);
            var user = UserGetByEmailUserCase.GetUser(email);
            return CreateGetWebResponse(user);
        }
    }
}
