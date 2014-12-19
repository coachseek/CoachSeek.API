using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Domain.Commands
{
    public class UserAssociateWithBusinessCommand
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid BusinessId { get; set; }

        public string BusinessName { get; set; } // Debug
    }
}
