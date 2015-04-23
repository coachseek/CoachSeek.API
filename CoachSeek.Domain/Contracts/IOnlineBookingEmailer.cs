using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Contracts
{
    public interface IOnlineBookingEmailer
    {
        void SendEmailToCoach(BookingData booking);
        void SendEmailToCustomer(BookingData booking);
    }
}
