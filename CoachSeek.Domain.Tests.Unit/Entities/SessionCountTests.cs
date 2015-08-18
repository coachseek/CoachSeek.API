using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionCountTests
    {
        [Test]
        public void SessionCountCreationTests()
        {
            SessionCountCreationFailure(-1000);
            SessionCountCreationFailure(-50);
            SessionCountCreationFailure(-5);
            SessionCountCreationFailure(-3);
            SessionCountCreationFailure(-2);
            SessionCountCreationFailure(-1);
            SessionCountCreationFailure(0);

            SessionCountCreationSuccess(1);
            SessionCountCreationSuccess(2);
            SessionCountCreationSuccess(3);
            SessionCountCreationSuccess(5);
            SessionCountCreationSuccess(10);
            SessionCountCreationSuccess(100);
            SessionCountCreationSuccess(1000);
        }


        private void SessionCountCreationSuccess(int inputCount)
        {
            var count = new SessionCount(inputCount);
            Assert.That(count, Is.Not.Null);
            Assert.That(count.Count, Is.EqualTo(inputCount));
        }

        private void SessionCountCreationFailure(int inputCount)
        {
            try
            {
                var times = new SessionCount(inputCount);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<SessionCountInvalid>());
            }
        }
    }
}
