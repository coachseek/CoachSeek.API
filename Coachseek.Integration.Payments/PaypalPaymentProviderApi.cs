﻿using System.IO;
using System.Net;
using System.Text;
using CoachSeek.Common;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Payments
{
    public class PaypalPaymentProviderApi : PaymentProviderApiBase
    {
        public override string SandboxUrl { get { return "https://www.sandbox.paypal.com/cgi-bin/webscr"; } }
        public override string LiveUrl { get { return "https://www.paypal.com/cgi-bin/webscr"; } }


        public PaypalPaymentProviderApi(bool isPaymentEnabled) 
            : base(isPaymentEnabled)
        { }


        public override bool VerifyPayment(PaymentProcessingMessage message)
        {
            var request = (HttpWebRequest)WebRequest.Create(Url);
            var formDataNotify = string.Format("{0}&cmd=_notify-validate", message.Contents);
            PreparePostRequest(request);
            SendData(formDataNotify, request);
            var response = HandleRawResponse(request);
            var payload = response.Payload.ToString();

            return payload == "VERIFIED";
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
                        //return new HttpResponse(status, DeserialiseErrors(errors));
                        return new HttpResponse(status);
                    }
                }
            }
        }

        //private object DeserialiseErrors(string errors)
        //{
        //    try
        //    {
        //        return JsonConvert.DeserializeObject<ApiApplicationError[]>(errors);
        //    }
        //    catch (JsonSerializationException)
        //    {
        //        return JsonConvert.DeserializeObject<ApiApplicationError>(errors);
        //    }
        //}
    }
}
