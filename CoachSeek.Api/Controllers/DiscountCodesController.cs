using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class DiscountCodesController : BaseController
    {
        public IDiscountCodeAddUseCase DiscountCodeAddUseCase { get; set; }
        public IDiscountCodeUpdateUseCase DiscountCodeUpdateUseCase { get; set; }

        public DiscountCodesController(IDiscountCodeAddUseCase discountCodeAddUseCase,
                                       IDiscountCodeUpdateUseCase discountCodeUpdateUseCase)
        {
            DiscountCodeAddUseCase = discountCodeAddUseCase;
            DiscountCodeUpdateUseCase = discountCodeUpdateUseCase;
        }

        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostAsync([FromBody]ApiDiscountCodeSaveCommand discountCode)
        {
            return discountCode.IsNew() ? await AddDiscountCodeAsync(discountCode) : await UpdateDiscountCodeAsync(discountCode);
        }

        private async Task<HttpResponseMessage> AddDiscountCodeAsync(ApiDiscountCodeSaveCommand discountCode)
        {
            var command = DiscountCodeAddCommandConverter.Convert(discountCode);
            DiscountCodeAddUseCase.Initialise(Context);
            var response = await DiscountCodeAddUseCase.AddDiscountCodeAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> UpdateDiscountCodeAsync(ApiDiscountCodeSaveCommand discountCode)
        {
            var command = DiscountCodeUpdateCommandConverter.Convert(discountCode);
            DiscountCodeUpdateUseCase.Initialise(Context);
            var response = await DiscountCodeUpdateUseCase.UpdateDiscountCodeAsync(command);
            return CreatePostWebResponse(response);
        }
    }
}
