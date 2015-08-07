using CoachSeek.Domain.Entities;
using Coachseek.Integration.Contracts.Interfaces;

namespace CoachSeek.Application.Services.Emailing
{
    public class NullEmailer : IEmailer
    {
        public bool Send(Email email)
        {
            // Doesn't do anything.
            return true;
        }
    }
}
