namespace CoachSeek.Domain.Commands
{
    public class BusinessPaymentCommand
    {
        public string Currency { get; set; }
        public bool IsOnlinePaymentEnabled { get; set; }
        public bool ForceOnlinePayment { get; set; }
        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }
        public bool UseProRataPricing { get; set; }
    }
}
