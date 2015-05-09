using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Tests.Unit.Fakes
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
