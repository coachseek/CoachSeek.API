using Coachseek.Integration.Contracts.Models;

namespace Coachseek.Integration.Contracts.Interfaces
{
    public interface IEmailer
    {
        void Send(Email email);
    }
}
