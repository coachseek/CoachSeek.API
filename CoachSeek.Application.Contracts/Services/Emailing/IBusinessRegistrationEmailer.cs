using CoachSeek.Application.Contracts.Models;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Services.Emailing
{
    public interface IBusinessRegistrationEmailer
    {
        void Initialise(ApplicationContext context);
        void SendEmail(RegistrationData registration);
    }
}
