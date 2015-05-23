using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Services.Emailing;
using Coachseek.Integration.Emailing.Amazon;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.Services.Emailing
{
    [TestFixture]
    public class EmailerFactoryTests
    {
        [Test]
        public void GivenIsEmailingEnabledIsFalse_WhenCreateEmailer_ThenReturnNullEmailer()
        {
            var context = new EmailContext(false, true, "", null);
            var emailer = EmailerFactory.CreateEmailer(true, context);
            Assert.That(emailer, Is.InstanceOf<NullEmailer>());
        }

        [Test]
        public void GivenIsTestingAndForceEmail_WhenCreateEmailer_ThenReturnProductionEmailer()
        {
            var context = new EmailContext(true, true, "", null);
            var emailer = EmailerFactory.CreateEmailer(true, context);
            Assert.That(emailer, Is.InstanceOf<AmazonEmailer>());
        }

        [Test]
        public void GivenIsTestingAndNotForceEmail_WhenCreateEmailer_ThenReturnTestingEmailer()
        {
            var context = new EmailContext(true, false, "", null);
            var emailer = EmailerFactory.CreateEmailer(true, context);
            Assert.That(emailer, Is.InstanceOf<NullEmailer>());
        }

        [Test]
        public void GivenNotIsTestingAndForceEmail_WhenCreateEmailer_ThenReturnProductionEmailer()
        {
            var context = new EmailContext(true, true, "", null);
            var emailer = EmailerFactory.CreateEmailer(false, context);
            Assert.That(emailer, Is.InstanceOf<AmazonEmailer>());
        }

        [Test]
        public void GivenNotIsTestingAndNotForceEmail_WhenCreateEmailer_ThenReturnProductionEmailer()
        {
            var context = new EmailContext(true, false, "", null);
            var emailer = EmailerFactory.CreateEmailer(false, context);
            Assert.That(emailer, Is.InstanceOf<AmazonEmailer>());
        }
    }
}