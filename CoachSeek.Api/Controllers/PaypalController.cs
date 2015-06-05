using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CoachSeek.Application.Services.Emailing;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Models;
using Newtonsoft.Json;

namespace CoachSeek.Api.Controllers
{
    public class PaypalController : BaseController
    {
        private const string PAYPAL_SANDBOX_URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        private const string PAYPAL_LIVE_URL = "https://www.paypal.com/cgi-bin/webscr";
        private const string PAYPAL = "PayPal";


        public IPaymentProcessingQueueClient PaymentProcessingQueueClient { get; set; }


        public PaypalController(IPaymentProcessingQueueClient paymentProcessingQueueClient)
        {
            PaymentProcessingQueueClient = paymentProcessingQueueClient;
        }


        public string PaypalUrl { get { return IsPaymentEnabled ? PAYPAL_LIVE_URL : PAYPAL_SANDBOX_URL; } }


        [AllowAnonymous]
        public HttpResponseMessage Post()
        {
            var task = Request.Content.ReadAsStringAsync();
            var formData = task.Result;

            //LogFormData(formData);

            var message = new PaymentProcessingMessage(PAYPAL, formData);
            PaymentProcessingQueueClient.PushPaymentProcessingMessageOntoQueue(message);

            //var request = (HttpWebRequest)WebRequest.Create(PaypalUrl);

            //var formDataNotify = string.Format("{0}&cmd=_notify-validate", formData);
            //PreparePostRequest(request);
            //SendData(formDataNotify, request);
            //var response = HandleRawResponse(request);
            //var payload = response.Payload.ToString();
            //LogResponse(payload);

            //if (payload == "VERIFIED")
            //{
            //    var message = new PaymentProcessingMessage(PAYPAL, formData);
            //    PaymentProcessingQueueClient.PushPaymentProcessingMessageOntoQueue(message);

            //    // check that Payment_status=Completed
            //    // check that Txn_id has not been previously processed
            //    // check that Receiver_email is your Primary PayPal email
            //    // check that Payment_amount/Payment_currency are correct
            //    // process payment
            //}
            //else
            //{
            //    // Log for manual investigation.
            //}

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void LogFormData(string formData)
        {
            var emailer = EmailerFactory.CreateEmailer(false, Context.EmailContext);
            var email = new Email(EmailSender, "olaf@coachseek.com", "API PayPal receive IPN", formData);
            var successful = emailer.Send(email);
        }

        private void LogResponse(string response)
        {
            var emailer = EmailerFactory.CreateEmailer(false, Context.EmailContext);
            var email = new Email(EmailSender, "olaf@coachseek.com", "API PayPal receive verification", response);
            var successful = emailer.Send(email);
        }

        private static void PreparePostRequest(HttpWebRequest request)
        {
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
        }

        private static void SendData(string json, HttpWebRequest request)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(json);

            var newStream = request.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
        }

        private static HttpResponse HandleResponse<TResponse>(HttpWebRequest request)
        {
            try
            {
                request.Timeout = 200000;
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<TResponse>(content);

                return new HttpResponse(status, obj);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                    return new HttpResponse(HttpStatusCode.RequestTimeout);

                var status = ((HttpWebResponse)ex.Response).StatusCode;
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var errors = reader.ReadToEnd();
                        return new HttpResponse(status, DeserialiseErrors(errors));
                    }
                }
            }
        }

        private static HttpResponse HandleRawResponse(HttpWebRequest request)
        {
            try
            {
                request.Timeout = 200000;
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();

                return new HttpResponse(status, content);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                    return new HttpResponse(HttpStatusCode.RequestTimeout);

                var status = ((HttpWebResponse)ex.Response).StatusCode;
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var errors = reader.ReadToEnd();
                        return new HttpResponse(status, DeserialiseErrors(errors));
                    }
                }
            }
        }

        private static object DeserialiseErrors(string errors)
        {
            try
            {
                return JsonConvert.DeserializeObject<ApplicationError[]>(errors);
            }
            catch (JsonSerializationException)
            {
                return JsonConvert.DeserializeObject<ApplicationError>(errors);
            }
        }
    }

    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public object Payload { get; private set; }

        public HttpResponse(HttpStatusCode statusCode, object payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }

    public class ApplicationError
    {
        public string field { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string data { get; set; }
    }
}
