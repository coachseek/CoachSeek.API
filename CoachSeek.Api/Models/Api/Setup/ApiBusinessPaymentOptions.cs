using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiBusinessPaymentOptions
    {
        [Required]
        public string Currency { get; set; }
        [Required]
        public bool? IsOnlinePaymentEnabled { get; set; }
        public bool? ForceOnlinePayment { get; set; }
        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }
        [Required]
        public bool? UseProRataPricing { get; set; }
    }
}