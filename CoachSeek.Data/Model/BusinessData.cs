using System;

namespace CoachSeek.Data.Model
{
    public class BusinessData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Sport { get; set; }
        public DateTime CreatedOn { get; set; }
        public BusinessPaymentData Payment { get; set; }

        public BusinessData()
        {
            Payment = new BusinessPaymentData();
        }
    }
}
