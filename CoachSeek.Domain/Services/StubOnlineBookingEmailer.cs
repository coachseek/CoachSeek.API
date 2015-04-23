using CoachSeek.Data.Model;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Services
{
    public class StubOnlineBookingEmailer : IOnlineBookingEmailer
    {
        public bool WasSendEmailToCoachCalled;
        public bool WasSendEmailToCustomerCalled;
        public BookingData PassedInBookingData;

        
        public void SendEmailToCoach(BookingData booking)
        {
            WasSendEmailToCoachCalled = true;
            PassedInBookingData = booking;
        }

        public void SendEmailToCustomer(BookingData booking)
        {
            WasSendEmailToCustomerCalled = true;
            PassedInBookingData = booking;
        }
    }
}
