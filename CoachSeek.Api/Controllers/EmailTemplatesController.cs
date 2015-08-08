using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class EmailTemplatesController : BaseController
    {
        public IEmailTemplateGetAllUseCase EmailTemplateGetAllUseCase { get; set; }
        public IEmailTemplateGetByTypeUseCase EmailTemplateGetByTypeUseCase { get; set; }
        public IEmailTemplateUpdateUseCase EmailTemplateUpdateUseCase { get; set; }
        public IEmailTemplateDeleteUseCase EmailTemplateDeleteUseCase { get; set; }

        public EmailTemplatesController(IEmailTemplateGetAllUseCase emailTemplateGetAllUseCase,
                                        IEmailTemplateGetByTypeUseCase emailTemplateGetByTypeUseCase,
                                        IEmailTemplateUpdateUseCase emailTemplateUpdateUseCase,
                                        IEmailTemplateDeleteUseCase emailTemplateDeleteUseCase)
        {
            EmailTemplateGetAllUseCase = emailTemplateGetAllUseCase;
            EmailTemplateGetByTypeUseCase = emailTemplateGetByTypeUseCase;
            EmailTemplateUpdateUseCase = emailTemplateUpdateUseCase;
            EmailTemplateDeleteUseCase = emailTemplateDeleteUseCase;
        }


        // GET: EmailTemplates
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            EmailTemplateGetAllUseCase.Initialise(Context);
            var response = EmailTemplateGetAllUseCase.GetEmailTemplates();
            return CreateGetWebResponse(response);
        }

        // GET: EmailTemplates/OnlineBookingCustomerSession
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(string id)
        {
            EmailTemplateGetByTypeUseCase.Initialise(Context);
            var response = EmailTemplateGetByTypeUseCase.GetEmailTemplate(id);
            return CreateGetWebResponse(response);
        }

        // POST: EmailTemplates/OnlineBookingCustomerSession
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post(string id, [FromBody] ApiEmailTemplateSaveCommand apiCommand)
        {
            apiCommand.Type = id;
            var command = EmailTemplateUpdateCommandConverter.Convert(apiCommand);
            EmailTemplateUpdateUseCase.Initialise(Context);
            var response = EmailTemplateUpdateUseCase.UpdateEmailTemplate(command);
            return CreatePostWebResponse(response);
        }

        // DELETE: EmailTemplates/OnlineBookingCustomerSession
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Delete(string id)
        {
            EmailTemplateDeleteUseCase.Initialise(Context);
            var response = EmailTemplateDeleteUseCase.DeleteEmailTemplate(id);
            return CreateDeleteWebResponse(response);
        }
    }
}