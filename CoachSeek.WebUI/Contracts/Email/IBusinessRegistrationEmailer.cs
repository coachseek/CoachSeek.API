using CoachSeek.WebUI.Models;

namespace CoachSeek.WebUI.Contracts.Email
{
    public interface IBusinessRegistrationEmailer
    {
        void SendEmail(Business business);
    }
}
