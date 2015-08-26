using CoachSeek.Domain.Entities;
using Coachseek.Integration.Contracts.Emailing.Interfaces;

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
