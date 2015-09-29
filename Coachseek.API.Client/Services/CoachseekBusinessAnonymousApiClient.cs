using System.Net;
using CoachSeek.Common;

namespace Coachseek.API.Client.Services
{
    public class CoachseekBusinessAnonymousApiClient : CoachseekAnonymousApiClient
    {
        private string BusinessDomain { get; set; }

        protected CoachseekBusinessAnonymousApiClient(string businessDomain,
                                                      string scheme = "https",
                                                      ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(scheme, dataFormat)
        {
            BusinessDomain = businessDomain;
        }


        protected override void ModifyRequest(HttpWebRequest request)
        {
            SetBusinessDomainHeader(request);
        }


        private void SetBusinessDomainHeader(WebRequest request)
        {
            if (BusinessDomain != null)
                request.Headers["Business-Domain"] = BusinessDomain;
        }
    }
}
