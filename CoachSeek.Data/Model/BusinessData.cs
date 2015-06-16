using System;

namespace CoachSeek.Data.Model
{
    public class BusinessData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Currency { get; set; }

        public bool IsOnlinePaymentEnabled { get; set; }
        public bool ForceOnlinePayment { get; set; }
        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }
    }
}
