using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiDiscountCodeSaveCommand : ApiSaveCommand
    {
        [Required, StringLength(8, MinimumLength = 3)]
        public string Code { get; set; }
        [Required, Range(1, 100)]
        public int DiscountPercent { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
}