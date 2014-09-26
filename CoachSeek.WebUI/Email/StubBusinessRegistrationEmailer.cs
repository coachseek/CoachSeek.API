using CoachSeek.WebUI.Contracts.Email;
using CoachSeek.WebUI.Models;

namespace CoachSeek.WebUI.Email
{
    public class StubBusinessRegistrationEmailer : IBusinessRegistrationEmailer
    {
        public bool WasSendEmailCalled;

        public void SendEmail(Business business)
        {
            WasSendEmailCalled = true;
        }
    }
}