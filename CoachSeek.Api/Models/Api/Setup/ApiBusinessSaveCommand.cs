using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiBusinessSaveCommand 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ApiBusinessPaymentOptions Payment { get; set; }
    }
}