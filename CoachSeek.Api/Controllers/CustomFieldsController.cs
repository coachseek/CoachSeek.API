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
using CoachSeek.Common;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Api.Controllers
{
    public class CustomFieldsController : BaseController
    {
        public ICustomFieldGetByIdUseCase CustomFieldGetByIdUseCase { get; set; }
        public ICustomFieldGetByTypeAndKeyUseCase CustomFieldGetByTypeAndKeyUseCase { get; set; }
        public ICustomFieldAddUseCase CustomFieldAddUseCase { get; set; }
        public ICustomFieldUpdateUseCase CustomFieldUpdateUseCase { get; set; }
        public ICustomFieldDeleteUseCase CustomFieldDeleteUseCase { get; set; }
        public ICustomFieldUseCaseExecutor CustomFieldUseCaseExecutor { get; set; }

        public CustomFieldsController(ICustomFieldGetByIdUseCase customFieldGetByIdUseCase,
                                      ICustomFieldGetByTypeAndKeyUseCase customFieldGetByTypeAndKeyUseCase,
                                      ICustomFieldAddUseCase customFieldAddUseCase,
                                      ICustomFieldUpdateUseCase customFieldUpdateUseCase,
                                      ICustomFieldDeleteUseCase customFieldDeleteUseCase,
                                      ICustomFieldUseCaseExecutor customFieldUseCaseExecutor)
        {
            CustomFieldGetByIdUseCase = customFieldGetByIdUseCase;
            CustomFieldGetByTypeAndKeyUseCase = customFieldGetByTypeAndKeyUseCase;
            CustomFieldAddUseCase = customFieldAddUseCase;
            CustomFieldUpdateUseCase = customFieldUpdateUseCase;
            CustomFieldDeleteUseCase = customFieldDeleteUseCase;
            CustomFieldUseCaseExecutor = customFieldUseCaseExecutor;
        }


        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            CustomFieldGetByIdUseCase.Initialise(Context);
            var response = await CustomFieldGetByIdUseCase.GetCustomFieldAsync(id);
            return CreateGetWebResponse(response);
        }

        [BasicAuthenticationOrAnonymous]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(string type = null, string key = null)
        {
            CustomFieldGetByTypeAndKeyUseCase.Initialise(Context);
            var response = await CustomFieldGetByTypeAndKeyUseCase.GetCustomFieldsByTypeAndKeyAsync(type, key);
            return CreateGetWebResponse(response);
        }

        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostAsync([FromBody]ApiCustomFieldSaveCommand customField)
        {
            return customField.IsNew() ? await AddCustomFieldAsync(customField) : await UpdateCustomFieldAsync(customField);
        }

        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        public async Task<HttpResponseMessage> PostAsync(Guid id, [FromBody] dynamic apiCommand)
        {
            apiCommand.TemplateId = id;
            ICommand command = DomainCommandConverter.Convert(apiCommand);
            var response = await CustomFieldUseCaseExecutor.ExecuteForAsync(command, Context);
            return CreatePostWebResponse(response);
        }

        //[BasicAuthentication]
        //[BusinessAuthorize(Role.BusinessAdmin)]
        //public async Task<HttpResponseMessage> DeleteAsync(string type, string key)
        //{
        //    CustomFieldDeleteUseCase.Initialise(Context);
        //    var response = await CustomFieldDeleteUseCase.DeleteCustomFieldAsync(type, key);
        //    return CreateDeleteWebResponse(response);
        //}


        private async Task<HttpResponseMessage> AddCustomFieldAsync(ApiCustomFieldSaveCommand customField)
        {
            var command = CustomFieldAddCommandConverter.Convert(customField);
            CustomFieldAddUseCase.Initialise(Context);
            var response = await CustomFieldAddUseCase.AddCustomFieldAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> UpdateCustomFieldAsync(ApiCustomFieldSaveCommand customField)
        {
            var command = CustomFieldUpdateCommandConverter.Convert(customField);
            CustomFieldUpdateUseCase.Initialise(Context);
            var response = await CustomFieldUpdateUseCase.UpdateCustomFieldAsync(command);
            return CreatePostWebResponse(response);
        }
    }
}
