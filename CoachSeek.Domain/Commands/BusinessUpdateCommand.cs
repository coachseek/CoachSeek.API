using System;

namespace CoachSeek.Domain.Commands
{
    public class BusinessUpdateCommand
    {
        // Business Id is not included because we will be in a business context anyway.

        public string Name { get; set; }
        public string Currency { get; set; }

        public bool IsOnlinePaymentEnabled { get; set; }
        public bool ForceOnlinePayment { get; set; }
        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }
    }
}
