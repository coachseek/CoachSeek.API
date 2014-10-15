using CoachSeek.Data.Model;
using CoachSeek.Services.Contracts.Email;

namespace CoachSeek.Services.Email
{
    public class StubBusinessRegistrationEmailer : IBusinessRegistrationEmailer
    {
        public bool WasSendEmailCalled;
        public BusinessData PassedInBusinessData;

        public void SendEmail(BusinessData business)
        {
            WasSendEmailCalled = true;
            PassedInBusinessData = business;
        }
    }
}