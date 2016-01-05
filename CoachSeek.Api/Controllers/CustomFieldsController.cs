using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;

namespace CoachSeek.Api.Controllers
{
    public class CustomFieldsController : BaseController
    {
        public ICustomFieldGetByTypeAndKeyUseCase CustomFieldGetByTypeAndKeyUseCase { get; set; }
        public ICustomFieldAddUseCase CustomFieldAddUseCase { get; set; }
        public ICustomFieldUpdateUseCase CustomFieldUpdateUseCase { get; set; }
        public ICustomFieldDeleteUseCase CustomFieldDeleteUseCase { get; set; }

        public CustomFieldsController(ICustomFieldGetByTypeAndKeyUseCase customFieldGetByTypeAndKeyUseCase,
                                      ICustomFieldAddUseCase customFieldAddUseCase,
                                      ICustomFieldDeleteUseCase customFieldDeleteUseCase)
        {
            CustomFieldGetByTypeAndKeyUseCase = customFieldGetByTypeAndKeyUseCase;
            CustomFieldAddUseCase = customFieldAddUseCase;
            CustomFieldDeleteUseCase = customFieldDeleteUseCase;
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
        public async Task<HttpResponseMessage> DeleteAsync(string type, string key)
        {
            CustomFieldDeleteUseCase.Initialise(Context);
            var response = await CustomFieldDeleteUseCase.DeleteCustomFieldAsync(type, key);
            return CreateDeleteWebResponse(response);
        }


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
