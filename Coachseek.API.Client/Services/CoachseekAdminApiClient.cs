using System.Web;
using Coachseek.API.Client.Models;
using CoachSeek.Common;

namespace Coachseek.API.Client.Services
{
    public class CoachseekAdminApiClient : CoachseekAuthenticatedApiClient
    {
        const string AdminUserName = "userZvFXUEmjht1hFJGn+H0YowMqO+5u5tEI";
        const string AdminPassword = "passYBoVaaWVp1W9ywZOHK6E6QXFh3z3+OUf";


        public CoachseekAdminApiClient(string scheme = "https", ApiDataFormat dataFormat = ApiDataFormat.Json)
            : base(AdminUserName, AdminPassword, scheme, dataFormat)
        {
            BaseUrl = BaseUrl + "/Admin";
        }


        //public void UnsubscribeEmailAddress(string emailAddress)
        //{
        //    var relativeUrl = string.Format("Email/Unsubscribe?email={0}", HttpUtility.UrlEncode(emailAddress));
        //    var response = GetAsync<string, ApiApplicationError[]>(relativeUrl);
        //}
    }
}
