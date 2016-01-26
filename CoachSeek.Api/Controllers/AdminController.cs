using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Admin;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Api.Controllers
{
    public class AdminController : BaseController
    {
        public IEmailUnsubscribeUseCase EmailUnsubscribeUseCase { get; set; }
        public IEmailIsUnsubscribedUseCase EmailIsUnsubscribedUseCase { get; set; }
        public IUserGetByEmailUserCase UserGetByEmailUserCase { get; set; }
        public IAdminUseCaseExecutor AdminUseCaseExecutor { get; set; }


        public AdminController(IEmailUnsubscribeUseCase emailUnsubscribeUseCase,
                               IEmailIsUnsubscribedUseCase emailIsUnsubscribedUseCase,
                               IUserGetByEmailUserCase userGetByEmailUserCase,
                               IAdminUseCaseExecutor adminUseCaseExecutor)
        {
            EmailUnsubscribeUseCase = emailUnsubscribeUseCase;
            EmailIsUnsubscribedUseCase = emailIsUnsubscribedUseCase;
            UserGetByEmailUserCase = userGetByEmailUserCase;
            AdminUseCaseExecutor = adminUseCaseExecutor;
        }


        protected AdminApplicationContext AdminContext
        {
            get
            {
                return new AdminApplicationContext(UserContext,
                                                   EmailContext,
                                                   BusinessRepository,
                                                   LogRepository,
                                                   IsTesting);
            }
        }


        // GET: Admin/Email/Unsubscribe?email=olaf@coachseek.com
        [Route("Admin/Email/Unsubscribe")]
        [BasicAdminAuthentication]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Unsubscribe(string email)
        {
            EmailUnsubscribeUseCase.Initialise(AdminContext);
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
            EmailIsUnsubscribedUseCase.Initialise(AdminContext);
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
        public async Task<HttpResponseMessage> GetUserAsync(string email)
        {
            UserGetByEmailUserCase.Initialise(AdminContext);
            var user = await UserGetByEmailUserCase.GetUserAsync(email);
            return CreateGetWebResponse(user);
        }

        [Route("Admin/Businesses/{id}")]
        [BasicAdminAuthentication]
        [BusinessAuthorize]
        [CheckModelForNull]
        public async Task<HttpResponseMessage> PostAsync(Guid id, [FromBody] dynamic apiCommand)
        {
            apiCommand.BusinessId = id;
            ICommand command = DomainCommandConverter.Convert(apiCommand);
            var response = await AdminUseCaseExecutor.ExecuteForAsync(command, AdminContext);
            return CreatePostWebResponse(response);
        }
    }
}
