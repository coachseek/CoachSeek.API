using System;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class BusinessSetUseProRataPricingCommand : ICommand
    {
        public bool UseProRataPricing { get; set; }
    }
}
