using CoachSeek.Data.Model;

namespace CoachSeek.Services.Contracts.Email
{
    public interface IBusinessRegistrationEmailer
    {
        void SendEmail(RegistrationData registration);
    }
}
