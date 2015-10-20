using System;

namespace CoachSeek.Data.Model
{
    public class BusinessData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Sport { get; set; }
        public string SubscriptionPlan { get; set; }
        public DateTime AuthorisedUntil { get; set; }
        public BusinessPaymentData Payment { get; set; }
        public BusinessStatisticsData Statistics { get; set; }

        public BusinessData()
        {
            Payment = new BusinessPaymentData();
        }
    }
}
