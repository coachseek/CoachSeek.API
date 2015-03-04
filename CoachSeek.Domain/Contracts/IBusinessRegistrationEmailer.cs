using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Contracts
{
    public interface IBusinessRegistrationEmailer
    {
        void SendEmail(RegistrationData registration);
    }
}
