using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Domain.Commands
{
    public class BusinessAddCommand
    {
        public string Name { get; set; }
        public string Currency { get; set; }
    }
}
