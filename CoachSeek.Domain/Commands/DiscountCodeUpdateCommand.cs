using System;

namespace CoachSeek.Domain.Commands
{
    public class DiscountCodeUpdateCommand : DiscountCodeAddCommand
    {
        public Guid Id { get; set; }
    }
}
