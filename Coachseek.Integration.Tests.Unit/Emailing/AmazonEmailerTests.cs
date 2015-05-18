using Coachseek.Integration.Contracts.Models;
using Coachseek.Integration.Emailing.Amazon;
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


        private Email GivenRecipientIsUnsubscribed()
        {
            return new Email("", "", "", "", true);
        }


        private bool WhenTryAndSendEmail(Email email)
        {
            var emailer = new AmazonEmailer();
            return emailer.Send(email);
        }
    }
}
