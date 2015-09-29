using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Coachseek.API.Client.Models;
using CoachSeek.Common;
using Newtonsoft.Json;

namespace Coachseek.API.Client.Services
{
    public abstract class GenericApiClient
    {
        protected string BaseUrl { get; set; }
        private ApiDataFormat DataFormat { get; set; }


        protected GenericApiClient(string baseUrl, ApiDataFormat dataFormat = ApiDataFormat.Json)
        {
            BaseUrl = baseUrl;
            DataFormat = dataFormat;
        }


        public ApiResponse Get<TResponse>(string relativeUrl)
        {
            var request = CreateWebRequest(relativeUrl);
            ModifyRequest(request);
            return Get<TResponse>(request);
        }

        public ApiResponse Post(string json, string relativeUrl)
        {
            var request = CreateWebRequest(relativeUrl);
            ModifyRequest(request);
            return Post(request, json);
        }

        public ApiResponse Post<TResponse>(string json, string relativeUrl)
        {
            var request = CreateWebRequest(relativeUrl);
            ModifyRequest(request);
            return Post<TResponse>(request, json);
        }

        public ApiResponse Delete(string relativeUrl, string id)
        {
            var request = CreateWebRequest(relativeUrl, id);
            ModifyRequest(request);
            return Delete(request);
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

        protected virtual void ModifyRequest(HttpWebRequest request)
        {

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

        protected ApiResponse Post<TResponse>(HttpWebRequest request, string json)
        {
            PreparePostRequest(request);
            MakeAdditionalChangesToRequest(request);
            SendData(json, request);
            return HandleResponse<TResponse>(request);
        }

        protected ApiResponse Delete(HttpWebRequest request)
        {
            PrepareDeleteRequest(request);
            MakeAdditionalChangesToRequest(request);
            return HandleResponse(request);
        }

        protected virtual void MakeAdditionalChangesToClient(HttpClient client)
        {
            // Opportunity to override HTTP headers etc.
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



        protected async Task<ApiResponse> GetAsync<TResponse>(string relativeUrl)
        {
            using (var client = CreateWebClient())
            {
                SetRequestHeaders(client);
                var response = await client.GetAsync(relativeUrl);
                try
                {
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    var obj = await response.Content.ReadAsAsync<TResponse>();
                    return new ApiResponse(response.StatusCode, obj);
                }
                catch (Exception ex)
                {
                    if (response.StatusCode == HttpStatusCode.RequestTimeout)
                        return new ApiResponse(HttpStatusCode.RequestTimeout);

                    return new ApiResponse(response.StatusCode);

                    //var status = ((HttpWebResponse)ex.Response).StatusCode;
                    //using (var stream = ex.Response.GetResponseStream())
                    //{
                    //    using (var reader = new StreamReader(stream))
                    //    {
                    //        var errors = reader.ReadToEnd();
                    //        return new ApiResponse(status, DeserialiseErrors(errors));
                    //    }
                    //}
                }
            }
        }

        protected async Task<ApiResponse> PostAsync(string json, string relativeUrl)
        {
            using (var client = CreateWebClient())
            {
                SetRequestHeaders(client);
                var request = CreatePostRequest(json, relativeUrl);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return new ApiResponse(response.StatusCode);

                if (response.StatusCode == HttpStatusCode.RequestTimeout)
                    return new ApiResponse(HttpStatusCode.RequestTimeout);

                var obj = await response.Content.ReadAsStringAsync();

                return new ApiResponse(response.StatusCode);

                //using (var stream = ex..GetResponseStream())
                //{
                //    using (var reader = new StreamReader(stream))
                //    {
                //        var errors = reader.ReadToEnd();
                //        return new ApiResponse(status, DeserialiseErrors(errors));
                //    }
                //}
            }
        }

        protected async Task<ApiResponse> PostAsync<TResponse>(string json, string relativeUrl)
        {
            using (var client = CreateWebClient())
            {
                SetRequestHeaders(client);
                var request = CreatePostRequest(json, relativeUrl);
                var response = await client.SendAsync(request);
                try
                {
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    var obj = await response.Content.ReadAsAsync<TResponse>();
                    return new ApiResponse(response.StatusCode, obj);
                }
                catch (Exception ex)
                {
                    if (response.StatusCode == HttpStatusCode.RequestTimeout)
                        return new ApiResponse(HttpStatusCode.RequestTimeout);

                    return new ApiResponse(response.StatusCode);

                    //var status = ((HttpWebResponse)ex.Response).StatusCode;
                    //using (var stream = ex.Response.GetResponseStream())
                    //{
                    //    using (var reader = new StreamReader(stream))
                    //    {
                    //        var errors = reader.ReadToEnd();
                    //        return new ApiResponse(status, DeserialiseErrors(errors));
                    //    }
                    //}
                }
            }
        }


        private HttpClient CreateWebClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        protected void SetRequestHeaders(HttpClient client)
        {
            SetAcceptHeader(client);
            SetOtherRequestHeaders(client);
        }

        private void SetAcceptHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            if (DataFormat == ApiDataFormat.Xml)
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            else
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected virtual void SetOtherRequestHeaders(HttpClient client)
        {

        }

        private HttpRequestMessage CreatePostRequest(string json, string relativeUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, relativeUrl)
            {
                Content = new StringContent(json, Encoding.UTF8)
            };
            SetContentTypeHeader(request.Content);
            return request;
        }

        private void SetContentTypeHeader(HttpContent content)
        {
            if (DataFormat == ApiDataFormat.Xml)
                content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            else
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
