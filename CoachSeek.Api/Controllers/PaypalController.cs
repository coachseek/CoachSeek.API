using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.Services.Emailing;
using Coachseek.Integration.Contracts.Models;

namespace CoachSeek.Api.Controllers
{
    public class PaypalController : BaseController
    {
        [AllowAnonymous]
        public HttpResponseMessage Post()
        {
            var form = Request.Content.ReadAsStringAsync();

            var emailer = EmailerFactory.CreateEmailer(false, Context.EmailContext);

            var email = new Email(EmailSender, "olaf@coachseek.com", "API PayPal IPN test", form.Result);
            var successful = emailer.Send(email);

            return null;
        }
    }
}
