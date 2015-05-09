﻿using CoachSeek.Application.Services.Emailing;
using Coachseek.Integration.Emailing.Amazon;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.Services.Emailing
{
    [TestFixture]
    public class EmailerFactoryTests
    {
        [Test]
        public void GivenIsTestingAndForceEmail_WhenCreateEmailer_ThenReturnProductionEmailer()
        {
            var emailer = EmailerFactory.CreateEmailer(true, true);
            Assert.That(emailer, Is.InstanceOf<AmazonEmailer>());
        }

        [Test]
        public void GivenIsTestingAndNotForceEmail_WhenCreateEmailer_ThenReturnTestingEmailer()
        {
            var emailer = EmailerFactory.CreateEmailer(true, false);
            Assert.That(emailer, Is.InstanceOf<NullEmailer>());
        }

        [Test]
        public void GivenNotIsTestingAndForceEmail_WhenCreateEmailer_ThenReturnProductionEmailer()
        {
            var emailer = EmailerFactory.CreateEmailer(false, true);
            Assert.That(emailer, Is.InstanceOf<AmazonEmailer>());
        }

        [Test]
        public void GivenNotIsTestingAndNotForceEmail_WhenCreateEmailer_ThenReturnProductionEmailer()
        {
            var emailer = EmailerFactory.CreateEmailer(false, false);
            Assert.That(emailer, Is.InstanceOf<AmazonEmailer>());
        }
    }
}