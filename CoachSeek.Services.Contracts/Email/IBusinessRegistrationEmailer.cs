using CoachSeek.Domain.Data;

namespace CoachSeek.Services.Contracts.Email
{
    public interface IBusinessRegistrationEmailer
    {
        void SendEmail(BusinessData newbusinessData);
    }
}
