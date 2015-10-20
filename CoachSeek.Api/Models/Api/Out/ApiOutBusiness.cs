using System;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutBusiness : ApiOutBasicBusiness
    {
        public string SubscriptionPlan { get; set; }
        public DateTime AuthorisedUntil { get; set; }
        public ApiOutBusinessStatistics Statistics { get; set; }

        public ApiOutBusiness()
        {
            Statistics = new ApiOutBusinessStatistics();
        }
    }
}