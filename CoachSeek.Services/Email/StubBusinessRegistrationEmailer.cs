using CoachSeek.Data.Model;
using CoachSeek.Services.Contracts.Email;

namespace CoachSeek.Services.Email
{
    public class StubBusinessRegistrationEmailer : IBusinessRegistrationEmailer
    {
        public bool WasSendEmailCalled;
        public RegistrationData PassedInRegistrationData;

        public void SendEmail(RegistrationData registration)
        {
            WasSendEmailCalled = true;
            PassedInRegistrationData = registration;
        }
    }
}