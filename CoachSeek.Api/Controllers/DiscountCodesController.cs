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
    public class DiscountCodesController : BaseController
    {
        public IDiscountCodeGetAllUseCase DiscountCodeGetAllUseCase { get; set; }
        public IDiscountCodeAddUseCase DiscountCodeAddUseCase { get; set; }
        public IDiscountCodeUpdateUseCase DiscountCodeUpdateUseCase { get; set; }

        public DiscountCodesController(IDiscountCodeGetAllUseCase discountCodeGetAllUseCase, 
                                       IDiscountCodeAddUseCase discountCodeAddUseCase,
                                       IDiscountCodeUpdateUseCase discountCodeUpdateUseCase)
        {
            DiscountCodeGetAllUseCase = discountCodeGetAllUseCase;
            DiscountCodeAddUseCase = discountCodeAddUseCase;
            DiscountCodeUpdateUseCase = discountCodeUpdateUseCase;
        }

        // GET: DiscountCodes
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> GetAsync()
        {
            DiscountCodeGetAllUseCase.Initialise(Context);
            var response = await DiscountCodeGetAllUseCase.GetDiscountCodesAsync();
            return CreateGetWebResponse(response);
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
