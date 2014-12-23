using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Domain.Commands
{
    public class BusinessAddCommand
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}
