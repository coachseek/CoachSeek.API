using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Emailing.Amazon;
using Coachseek.Integration.Tests.Unit.Fakes;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Emailing
{
    [TestFixture]
    public class AmazonEmailerTests
    {
        [Test]
        public void GivenRecipientIsUnsubscribed_WhenTryAndSendEmail_ThenNoEmailWillBeSent()
        {
            var email = GivenRecipientIsUnsubscribed();
            var wasEmailSent = WhenTryAndSendEmail(email);
            Assert.That(wasEmailSent, Is.False);
        }


        private IUnsubscribedEmailAddressRepository GivenRecipientIsUnsubscribed()
        {
            return new StubUnsubscribedEmailAddressRepository {SetIsEmailAddressUnsubscribed = true};
        }


        private bool WhenTryAndSendEmail(IUnsubscribedEmailAddressRepository repository)
        {
            var emailer = new AmazonEmailer(repository);
            return emailer.Send(new BouncedEmail("", "", "", ""));
        }
    }
}
