using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;

namespace CoachSeek.Api.Controllers
{
    public class CustomFieldsController : BaseController
    {
        public ICustomFieldTemplateGetByIdUseCase CustomFieldTemplateGetByIdUseCase { get; set; }
        public ICustomFieldTemplatesGetByTypeAndKeyUseCase CustomFieldTemplatesGetByTypeAndKeyUseCase { get; set; }
        public ICustomFieldTemplateAddUseCase CustomFieldTemplateAddUseCase { get; set; }
        public ICustomFieldTemplateUpdateUseCase CustomFieldTemplateUpdateUseCase { get; set; }
        public ICustomFieldUseCaseExecutor CustomFieldUseCaseExecutor { get; set; }

        public CustomFieldsController(ICustomFieldTemplateGetByIdUseCase customFieldTemplateGetByIdUseCase,
                                      ICustomFieldTemplatesGetByTypeAndKeyUseCase customFieldTemplatesGetByTypeAndKeyUseCase,
                                      ICustomFieldTemplateAddUseCase customFieldTemplateAddUseCase,
                                      ICustomFieldTemplateUpdateUseCase customFieldTemplateUpdateUseCase,
                                      ICustomFieldUseCaseExecutor customFieldUseCaseExecutor)
        {
            CustomFieldTemplateGetByIdUseCase = customFieldTemplateGetByIdUseCase;
            CustomFieldTemplatesGetByTypeAndKeyUseCase = customFieldTemplatesGetByTypeAndKeyUseCase;
            CustomFieldTemplateAddUseCase = customFieldTemplateAddUseCase;
            CustomFieldTemplateUpdateUseCase = customFieldTemplateUpdateUseCase;
            CustomFieldUseCaseExecutor = customFieldUseCaseExecutor;
        }


        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            CustomFieldTemplateGetByIdUseCase.Initialise(Context);
            var response = await CustomFieldTemplateGetByIdUseCase.GetCustomFieldTemplateAsync(id);
            return CreateGetWebResponse(response);
        }

        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(string type = null, string key = null)
        {
            CustomFieldTemplatesGetByTypeAndKeyUseCase.Initialise(Context);
            var response = await CustomFieldTemplatesGetByTypeAndKeyUseCase.GetCustomFieldTemplatesByTypeAndKeyAsync(type, key);
            return CreateGetWebResponse(response);
        }

        //[BasicAuthentication]
        //[Authorize]
        //[CheckModelForNull]
        //[ValidateModelState]
        //public async Task<HttpResponseMessage> PostAddAsync([FromBody]ApiCustomFieldAddCommand customField)
        //{
        //    return await AddCustomFieldAsync(customField);
        //}

        //[BasicAuthentication]
        //[Authorize]
        //[CheckModelForNull]
        //[ValidateModelState]
        //public async Task<HttpResponseMessage> PostUpdateAsync([FromBody]ApiCustomFieldUpdateCommand customField)
        //{
        //    return await UpdateCustomFieldAsync(customField);
        //}

        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostAsync([FromBody]ApiCustomFieldSaveCommand customField)
        {
            return customField.IsNew() ? await AddCustomFieldAsync(customField) : await UpdateCustomFieldAsync(customField);
        }

        //[BasicAuthentication]
        //[BusinessAuthorize(Role.BusinessAdmin)]
        //[CheckModelForNull]
        //public async Task<HttpResponseMessage> PostAsync(Guid id, [FromBody] dynamic apiCommand)
        //{
        //    apiCommand.TemplateId = id;
        //    ICommand command = DomainCommandConverter.Convert(apiCommand);
        //    var response = await CustomFieldUseCaseExecutor.ExecuteForAsync(command, Context);
        //    return CreatePostWebResponse(response);
        //}


        private async Task<HttpResponseMessage> AddCustomFieldAsync(ApiCustomFieldSaveCommand customField)
        {
            var command = CustomFieldAddCommandConverter.Convert(customField);
            CustomFieldTemplateAddUseCase.Initialise(Context);
            var response = await CustomFieldTemplateAddUseCase.AddCustomFieldTemplateAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> UpdateCustomFieldAsync(ApiCustomFieldSaveCommand customField)
        {
            var command = CustomFieldUpdateCommandConverter.Convert(customField);
            CustomFieldTemplateUpdateUseCase.Initialise(Context);
            var response = await CustomFieldTemplateUpdateUseCase.UpdateCustomFieldTemplateAsync(command);
            return CreatePostWebResponse(response);
        }
    }
}
