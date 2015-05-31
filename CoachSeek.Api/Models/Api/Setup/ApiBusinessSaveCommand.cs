using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiBusinessSaveCommand 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Currency { get; set; }

        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }
    }
}