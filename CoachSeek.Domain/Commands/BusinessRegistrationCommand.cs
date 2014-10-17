using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Domain.Commands
{
    public class BusinessRegistrationCommand
    {
        [Required, StringLength(100)]
        public string BusinessName { get; set; }

        [Required]
        public BusinessRegistrantCommand Registrant { get; set; }
    }
}
