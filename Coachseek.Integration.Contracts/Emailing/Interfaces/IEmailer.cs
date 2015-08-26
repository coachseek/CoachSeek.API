using CoachSeek.Domain.Entities;

namespace Coachseek.Integration.Contracts.Emailing.Interfaces
{
    public interface IEmailer
    {
        bool Send(Email email);
    }
}
