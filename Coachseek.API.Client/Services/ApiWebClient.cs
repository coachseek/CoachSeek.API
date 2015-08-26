using System;
using System.IO;
using System.Net;
using System.Text;
using Coachseek.API.Client.Models;
using CoachSeek.Common;
using Newtonsoft.Json;

namespace Coachseek.API.Client.Services
{
    public abstract class ApiWebClient
    {
        private string BaseUrl { get; set; }
        private ApiDataFormat DataFormat { get; set; }


        protected ApiWebClient(string baseUrl, ApiDataFormat dataFormat = ApiDataFormat.Json)
        {
            BaseUrl = baseUrl;
            DataFormat = dataFormat;
        }

        protected ApiWebClient(string scheme, string host, int? port = null, ApiDataFormat dataFormat = ApiDataFormat.Json)
        {
            FormatBaseUrl(scheme, host, port);
            DataFormat = dataFormat;
        }

        private void FormatBaseUrl(string scheme, string host, int? port)
        {
            if (port.HasValue)
                BaseUrl = string.Format("{0}://{1}:{2}", scheme, host, port);
            else
                BaseUrl = string.Format("{0}://{1}", scheme, host);
        }


        protected HttpWebRequest CreateWebRequest(string relativeUrl)
        {
            var url = FormatUrl(relativeUrl);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        protected HttpWebRequest CreateWebRequest(string relativeUrl, string id)
        {
            var url = FormatUrl(relativeUrl, id);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        protected virtual Uri FormatUrl(string relativeUrl)
        {
            return new Uri(string.Format("{0}/{1}", BaseUrl, relativeUrl));
        }

        protected virtual Uri FormatUrl(string relativeUrl, string id)
        {
            return new Uri(string.Format("{0}/{1}/{2}", BaseUrl, relativeUrl, id));
        }

        protected ApiResponse Get<TResponse>(HttpWebRequest request)
        {
            PrepareGetRequest(request);
            MakeAdditionalChangesToRequest(request);
            return HandleResponse<TResponse>(request);
        }

        protected ApiResponse Post(HttpWebRequest request, string json)
        {
            PreparePostRequest(request);
            MakeAdditionalChangesToRequest(request);
            SendData(json, request);
            return HandleResponse(request);
        }

        //protected ApiResponse Post<TResponse>(HttpWebRequest request, string json)
        //{
        //    PreparePostRequest(request);
        //    MakeAdditionalChangesToRequest(request);
        //    SendData(json, request);
        //    return HandleResponse<TResponse>(request);
        //}

        protected ApiResponse Delete(HttpWebRequest request)
        {
            PrepareDeleteRequest(request);
            MakeAdditionalChangesToRequest(request);
            return HandleResponse(request);
        }

        protected virtual void MakeAdditionalChangesToRequest(HttpWebRequest request)
        {
            // Opportunity to override HTTP headers etc.
        }


        private void PrepareGetRequest(HttpWebRequest request)
        {
            request.Method = "GET";

            if (DataFormat == ApiDataFormat.Xml)
                request.Accept = "application/xml";
            else
                request.Accept = "application/json";
        }

        private void PreparePostRequest(HttpWebRequest request)
        {
            request.Method = "POST";

            if (DataFormat == ApiDataFormat.Xml)
            {
                request.Accept = "application/xml";
                request.ContentType = "application/xml";
            }
            else
            {
                request.Accept = "application/json";
                request.ContentType = "application/json";
            }
        }

        private void PrepareDeleteRequest(HttpWebRequest request)
        {
            request.Method = "DELETE";

            if (DataFormat == ApiDataFormat.Xml)
                request.Accept = "application/xml";
            else
                request.Accept = "application/json";
        }

        private static void SendData(string json, HttpWebRequest request)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(json);

            var newStream = request.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
        }

        private ApiResponse HandleResponse(HttpWebRequest request)
        {
            try
            {
                request.Timeout = 200000;
                var response = request.GetResponse();
                var status = ((HttpWebResponse)response).StatusCode;
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();

                return new ApiResponse(status, content);
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
