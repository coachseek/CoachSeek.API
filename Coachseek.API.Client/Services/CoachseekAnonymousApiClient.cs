using System;
using CoachSeek.Common;

namespace Coachseek.API.Client.Services
{
    public class CoachseekAnonymousApiClient : GenericApiClient
    {
        protected CoachseekAnonymousApiClient(string scheme = "https", ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(string.Empty, dataFormat)
        {
            BaseUrl = FormatBaseUrl(scheme);
        }


        private string FormatBaseUrl(string scheme)
        {
            if (scheme.ToLower() == "https")
                return Properties.Settings.Default.HttpsBaseUrl;
            if (scheme.ToLower() == "http")
                return Properties.Settings.Default.HttpBaseUrl;

            throw new InvalidOperationException("Scheme not supported.");
        }
    }
}
