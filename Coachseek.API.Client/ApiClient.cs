using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Coachseek.API.Client
{
    public static class ApiClient
    {
        const string AdminUserName = "userZvFXUEmjht1hFJGn+H0YowMqO+5u5tEI";
        const string AdminPassword = "passYBoVaaWVp1W9ywZOHK6E6QXFh3z3+OUf";

        private static string BaseUrl
        {
            get
            {
#if DEBUG
                return "https://localhost:44300";
#else
                return "https://api.coachseek.com";
#endif
            }
        }


        public static Response AnonymousGet<TResponse>(string relativeUrl, string businessDomain)
        {
            var http = CreateWebRequest(relativeUrl);
            SetBusinessDomainHeader(http, businessDomain);

            return Get<TResponse>(http);
        }

        public static Response AuthenticatedGet<TResponse>(string relativeUrl, string username, string password)
        {
            var http = CreateWebRequest(relativeUrl);
            SetBasicAuthHeader(http, username, password);

            return Get<TResponse>(http);
        }

        public static Response AdminAuthenticatedGet<TResponse>(string relativeUrl)
        {
            var http = CreateAdminWebRequest(relativeUrl);
            SetBasicAuthHeader(http, AdminUserName, AdminPassword);

            return Get<TResponse>(http);
        }

        public static Response AnonymousPost<TResponse>(string relativeUrl, string json)
        {
            var http = CreateWebRequest(relativeUrl);

            return Post<TResponse>(http, json);
        }

        public static Response AnonymousPostForBusiness<TResponse>(string relativeUrl, string json, string businessDomain)
        {
            var http = CreateWebRequest(relativeUrl);
            SetBusinessDomainHeader(http, businessDomain);

            return Post<TResponse>(http, json);
        }

        public static Response AuthenticatedPost<TResponse>(string relativeUrl, string json, string username, string password)
        {
            var http = CreateWebRequest(relativeUrl);
            SetBasicAuthHeader(http, username, password);

            return Post<TResponse>(http, json);
        }


        private static HttpWebRequest CreateWebRequest(string relativeUrl)
        {
            var url = FormatUrl(relativeUrl);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        private static Uri FormatUrl(string relativeUrl)
        {
            return new Uri(string.Format("{0}/{1}", BaseUrl, relativeUrl));
        }

        private static HttpWebRequest CreateAdminWebRequest(string relativeUrl)
        {
            var url = FormatAdminUrl(relativeUrl);
            return (HttpWebRequest)WebRequest.Create(url);
        }

        private static Uri FormatAdminUrl(string relativeUrl)
        {
            return new Uri(string.Format("{0}/Admin/{1}", BaseUrl, relativeUrl));
        }

        private static Response Get<TResponse>(HttpWebRequest request)
        {
            PrepareGetRequest(request);

            return HandleResponse<TResponse>(request);
        }

        private static void PrepareGetRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.Method = "GET";
        }

        private static void SetBusinessDomainHeader(WebRequest request, string domain)
        {
            if (domain != null)
                request.Headers["Business-Domain"] = domain;
        }

        private static void SetBasicAuthHeader(WebRequest request, string username, string password)
        {
            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }


        private static Response Post<TResponse>(HttpWebRequest request, string json)
        {
            PreparePostRequest(request);
            SendData(json, request);
            return HandleResponse<TResponse>(request);
        }

        private static void PreparePostRequest(HttpWebRequest request)
        {
            request.Accept = "application/json";
            request.ContentType = "application/json";
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

        private static Response HandleResponse<TResponse>(HttpWebRequest request)
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

                return new Response(status, obj);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                    return new Response(HttpStatusCode.RequestTimeout);

                var status = ((HttpWebResponse)ex.Response).StatusCode;
                using (var stream = ex.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var errors = reader.ReadToEnd();
                        return new Response(status, DeserialiseErrors(errors));
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
}
