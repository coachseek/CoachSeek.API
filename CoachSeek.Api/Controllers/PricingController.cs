using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class PricingController : BaseController
    {
        public IPriceGetUseCase PriceGetUseCase { get; set; }


        public PricingController(IPriceGetUseCase priceGetUseCase)
        {
            PriceGetUseCase = priceGetUseCase;
        }

        // POST: Prices
        [Route("Pricing/Enquiry")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostGetPriceAsync([FromBody]ApiPriceGetCommand priceGetCommand)
        {
            var command = PriceGetCommandConverter.Convert(priceGetCommand);
            PriceGetUseCase.Initialise(Context);
            var price = await PriceGetUseCase.GetPriceAsync(command);
            return CreatePostWebResponse(price);
        }
    }
}
