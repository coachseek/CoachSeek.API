using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.Tests.Unit.Fakes
{
    public class StubUnsubscribedEmailAddressRepository : IUnsubscribedEmailAddressRepository
    {
        public bool WasSaveCalled;
        public bool SetIsEmailAddressUnsubscribed;


        public bool Get(string emailAddress)
        {
            return SetIsEmailAddressUnsubscribed;
        }

        public void Save(string emailAddress)
        {
            WasSaveCalled = true;
        }
    }
}
