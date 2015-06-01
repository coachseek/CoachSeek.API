using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class StubBusinessRegistrationEmailer : IBusinessRegistrationEmailer
    {
        public bool WasInitialiseCalled;
        public bool WasSendEmailCalled;
        public RegistrationData PassedInRegistrationData;


        public void Initialise(ApplicationContext context)
        {
            WasInitialiseCalled = true;
        }

        public void SendEmail(RegistrationData registration)
        {
            WasSendEmailCalled = true;
            PassedInRegistrationData = registration;
        }
    }
}