using CoachSeek.Domain.Entities;

namespace Coachseek.Integration.Contracts.Interfaces
{
    public interface IEmailer
    {
        bool Send(Email email);
    }
}
