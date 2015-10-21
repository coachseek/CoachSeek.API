using CoachSeek.Application.Contracts.UseCases.Payments;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoachSeek.Api.Controllers
{
    public class PaypalController : BaseController
    {
        public IPaypalReceiveOnlinePaymentMessageUseCase PaypalReceiveOnlinePaymentMessageUseCase { get; set; }
        public IPaypalReceiveSubscriptionPaymentMessageUseCase PaypalReceiveSubscriptionPaymentMessageUseCase { get; set; }


        public PaypalController(IPaypalReceiveOnlinePaymentMessageUseCase paypalReceiveOnlinePaymentMessageUseCase)
        {
            PaypalReceiveOnlinePaymentMessageUseCase = paypalReceiveOnlinePaymentMessageUseCase;
        }

        //[Route("Paypal/Payment")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostAsync()
        {
            var content = await Request.Content.ReadAsStringAsync();
            await PaypalReceiveOnlinePaymentMessageUseCase.ReceiveAsync(content);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST: Paypal/Subscription
        [Route("Paypal/Subscription")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostSubscriptionAsync()
        {
            var content = await Request.Content.ReadAsStringAsync();
            await PaypalReceiveSubscriptionPaymentMessageUseCase.ReceiveAsync(content);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
