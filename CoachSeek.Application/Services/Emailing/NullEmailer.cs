using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Contracts.Models;

namespace CoachSeek.Application.Services.Emailing
{
    public class NullEmailer : IEmailer
    {
        public void Send(Email email)
        {
            // Doesn't do anything.
        }
    }
}
