using System;
using System.Linq;
using System.Text;
using System.Web;
using CoachSeek.Common;
using CoachSeek.Common.Services.Authentication;

namespace Coachseek.Logging.Contracts
{
    public class RequestLogMessage
    {
        private DateTime? _finishTime;

        private HttpContext Context { get; set; }

        public string HttpMethod { get { return Context.Request.HttpMethod; } }
        public string Url { get { return Context.Request.RawUrl; } }
        public DateTime StartTime { get { return Context.Timestamp; } }
        public DateTime FinishTime
        {
            get
            {
                _finishTime = _finishTime ?? DateTime.UtcNow;
                return _finishTime.Value;
            } 
        }
        public double Duration { get { return (FinishTime - StartTime).Milliseconds; } }
        public int StatusCode { get { return Context.Response.StatusCode; } }
        public string BusinessDomain { get { return GetBusinessDomain(Context.Request); } }
        public string UserLogin { get { return GetUserLogin(Context.Request); } }
        public string Data { get { return GetPostData(Context.Request); } }


        public RequestLogMessage(HttpContext context)
        {
            Context = context;
        }


        private string GetBusinessDomain(HttpRequest request)
        {
            if (request.Headers.AllKeys.Contains(Constants.BUSINESS_DOMAIN))
                return request.Headers[Constants.BUSINESS_DOMAIN];
            return null;
        }

        private string GetUserLogin(HttpRequest request)
        {
            const string BASIC_AUTH = "Basic ";
            if (!request.Headers.AllKeys.Contains(Constants.AUTHORIZATION))
                return null;
            var authorization = request.Headers[Constants.AUTHORIZATION];
            if (!authorization.StartsWith(BASIC_AUTH))
                return null;
            var authorizationParameter = authorization.Substring(BASIC_AUTH.Length);
            if (string.IsNullOrEmpty(authorizationParameter))
                return null;
            var credentials = BasicAuthenticationCredentialsExtractor.ExtractUsernameAndPassword(authorizationParameter);
            if (credentials == null)
                return null;
            return credentials.Item1;
        }

        private static string GetPostData(HttpRequest request)
        {
            if (request.TotalBytes > 0)
                return Encoding.Default.GetString(request.BinaryRead(request.TotalBytes));
            return string.Empty;
        }
    }
}
