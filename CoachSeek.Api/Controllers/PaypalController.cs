using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class PaypalController : BaseController
    {
        public IPaypalReceivePaymentMessageUseCase PaypalReceivePaymentMessageUseCase { get; set; }


        public PaypalController(IPaypalReceivePaymentMessageUseCase paypalReceivePaymentMessageUseCase)
        {
            PaypalReceivePaymentMessageUseCase = paypalReceivePaymentMessageUseCase;
        }


        [AllowAnonymous]
        public HttpResponseMessage Post()
        {
            var task = Request.Content.ReadAsStringAsync();

            PaypalReceivePaymentMessageUseCase.Receive(task.Result);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
