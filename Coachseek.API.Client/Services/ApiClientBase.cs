using System;
using System.IO;
using System.Net;
using System.Text;
using Coachseek.API.Client.Models;
using Newtonsoft.Json;

namespace Coachseek.API.Client.Services
{
    public abstract class ApiClientBase
    {
        protected HttpWebRequest CreateWebRequest(string relativeUrl, string scheme = "https")
        {
            var url = FormatUrl(relativeUrl, scheme);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        protected HttpWebRequest CreateWebRequest(string relativeUrl, Guid id, string scheme = "https")
        {
            var url = FormatUrl(relativeUrl, id, scheme);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        protected string FormatBaseUrl(string scheme = "https")
        {
            if (scheme.ToLower() == "https")
                return Properties.Settings.Default.HttpsBaseUrl;
            if (scheme.ToLower() == "http")
                return Properties.Settings.Default.HttpBaseUrl;

            throw new InvalidOperationException("Scheme not supported.");
        }

        protected virtual Uri FormatUrl(string relativeUrl, string scheme = "https")
        {
            return new Uri(string.Format("{0}/{1}", FormatBaseUrl(scheme), relativeUrl));
        }

        protected virtual Uri FormatUrl(string relativeUrl, Guid id, string scheme = "https")
        {
            return new Uri(string.Format("{0}/{1}/{2}", FormatBaseUrl(scheme), relativeUrl, id));
        }

        protected ApiResponse Get<TResponse>(HttpWebRequest request)
        {
            PrepareGetRequest(request);
            MakeAdditionalChangesToRequest(request);
            return HandleResponse<TResponse>(request);
        }

        protected ApiResponse Post<TResponse>(HttpWebRequest request, string json)
        {
            PreparePostRequest(request);
            MakeAdditionalChangesToRequest(request);
            SendData(json, request);
            return HandleResponse<TResponse>(request);
        }

        protected ApiResponse Delete<TResponse>(HttpWebRequest request)
        {
            PrepareDeleteRequest(request);
            MakeAdditionalChangesToRequest(request);
            return HandleResponse<TResponse>(request);
        }

        protected virtual void MakeAdditionalChangesToRequest(HttpWebRequest request)
        {
            // Opportunity to override HTTP headers etc.
        }


        private void PrepareGetRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.Method = "GET";
        }

        private void PreparePostRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";
        }

        private void PrepareDeleteRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.Method = "DELETE";
        }

        private static void SendData(string json, HttpWebRequest request)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(json);

            var newStream = request.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
        }

        private ApiResponse HandleResponse<TResponse>(HttpWebRequest request)
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

                return new ApiResponse(status, obj);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                    return new ApiResponse(HttpStatusCode.RequestTimeout);

                var status = ((HttpWebResponse)ex.Response).StatusCode;
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var errors = reader.ReadToEnd();
                        return new ApiResponse(status, DeserialiseErrors(errors));
                    }
                }
            }
        }

        private object DeserialiseErrors(string errors)
        {
            try
            {
                return JsonConvert.DeserializeObject<ApiApplicationError[]>(errors);
            }
            catch (JsonSerializationException)
            {
                return JsonConvert.DeserializeObject<ApiApplicationError>(errors);
            }
        }
    }
}
