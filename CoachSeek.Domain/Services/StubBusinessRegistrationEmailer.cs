using CoachSeek.Data.Model;
using IBusinessRegistrationEmailer = CoachSeek.Domain.Contracts.IBusinessRegistrationEmailer;

namespace CoachSeek.Domain.Services
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