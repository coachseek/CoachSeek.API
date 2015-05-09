using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Services.Emailing
{
    public interface IOnlineBookingEmailer
    {
        void SendEmailToCoach(BookingData booking);
        void SendEmailToCustomer(BookingData booking);
    }
}
