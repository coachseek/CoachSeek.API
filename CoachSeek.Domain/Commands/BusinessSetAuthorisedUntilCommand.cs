using System;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class BusinessSetAuthorisedUntilCommand : ICommand
    {
        public Guid BusinessId { get; set; }
        public DateTime AuthorisedUntil { get; set; }
    }
}
