using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class DateTests
    {
        [Test]
        public void DateCreationTests()
        {
            DateCreationFailure(null);
            DateCreationFailure("hello world!");
            DateCreationFailure("2014-00-00");
            DateCreationFailure("2014-15-39");
            DateCreationFailure("2014-15-09");
            DateCreationFailure("2014-11-39");
            DateCreationFailure("2014-02-30");
            DateCreationFailure("2014-02-29");      // Is not a leap year

            DateCreationSuccess("2014-01-01");
            DateCreationSuccess("2014-1-1", "2014-01-01");
            DateCreationSuccess("2014-06-06", 6);   // Check that we have month and day the right way round.
            DateCreationSuccess("2014-05-07", 5);   // Check that we have month and day the right way round.
            DateCreationSuccess("2014-07-05", 7);   // Check that we have month and day the right way round.
            DateCreationSuccess("2014-12-31");
            DateCreationSuccess("2012-02-29");      // Is a leap year
        }

        private void DateCreationSuccess(string inputDate)
        {
            var date = new Date(inputDate);
            Assert.That(date, Is.Not.Null);
            Assert.That(date.ToData(), Is.EqualTo(inputDate));
        }

        private void DateCreationSuccess(string inputDate, string expectedDate)
        {
            var date = new Date(inputDate);
            Assert.That(date, Is.Not.Null);
            Assert.That(date.ToData(), Is.EqualTo(expectedDate));
        }

        private void DateCreationSuccess(string inputDate, int expectedMonth)
        {
            var date = new Date(inputDate);
            Assert.That(date, Is.Not.Null);
            Assert.That(date.ToData(), Is.EqualTo(inputDate));
            Assert.That(date.Month, Is.EqualTo(expectedMonth));
        }

        private void DateCreationFailure(string inputDate)
        {
            try
            {
                var date = new Date(inputDate);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidDate>());
            }
        }

        [Test]
        public void DateIsAfterTests()
        {
            DateIsAfter("2015-01-17", "2015-01-18", false);
            DateIsAfter("2014-12-31", "2015-01-01", false);
            DateIsAfter("2015-01-01", "2015-02-01", false);

            DateIsAfter("2015-01-01", "2015-01-01", false);
            DateIsAfter("2015-01-18", "2015-01-18", false);

            DateIsAfter("2015-01-18", "2015-01-17", true);
            DateIsAfter("2015-01-01", "2014-12-31", true);
            DateIsAfter("2015-02-01", "2015-01-01", true);
        }

        private void DateIsAfter(string afterDateString, string beforeDateString, bool expected)
        {
            var afterDate = new Date(afterDateString);
            var beforeDate = new Date(beforeDateString);
            Assert.That(afterDate.IsAfter(beforeDate), Is.EqualTo(expected));
        }

        [Test]
        public void DateIsOnOrAfterTests()
        {
            DateIsOnOrAfter("2015-01-17", "2015-01-18", false);
            DateIsOnOrAfter("2014-12-31", "2015-01-01", false);
            DateIsOnOrAfter("2015-01-01", "2015-02-01", false);

            DateIsOnOrAfter("2015-01-01", "2015-01-01", true);
            DateIsOnOrAfter("2015-01-18", "2015-01-18", true);

            DateIsOnOrAfter("2015-01-18", "2015-01-17", true);
            DateIsOnOrAfter("2015-01-01", "2014-12-31", true);
            DateIsOnOrAfter("2015-02-01", "2015-01-01", true);
        }

        private void DateIsOnOrAfter(string afterDateString, string beforeDateString, bool expected)
        {
            var afterDate = new Date(afterDateString);
            var beforeDate = new Date(beforeDateString);
            Assert.That(afterDate.IsOnOrAfter(beforeDate), Is.EqualTo(expected));
        }

        [Test]
        public void DateIsOnOrBeforeTests()
        {
            DateIsOnOrBefore("2015-01-17", "2015-01-18", true);
            DateIsOnOrBefore("2014-12-31", "2015-01-01", true);
            DateIsOnOrBefore("2015-01-01", "2015-02-01", true);

            DateIsOnOrBefore("2015-01-01", "2015-01-01", true);
            DateIsOnOrBefore("2015-01-18", "2015-01-18", true);

            DateIsOnOrBefore("2015-01-18", "2015-01-17", false);
            DateIsOnOrBefore("2015-01-01", "2014-12-31", false);
            DateIsOnOrBefore("2015-02-01", "2015-01-01", false);
        }

        private void DateIsOnOrBefore(string beforeDateString, string afterDateString, bool expected)
        {
            var beforeDate = new Date(beforeDateString);
            var afterDate = new Date(afterDateString);
            Assert.That(beforeDate.IsOnOrBefore(afterDate), Is.EqualTo(expected));
        }

        [Test]
        public void DateIsBeforeTests()
        {
            DateIsBefore("2015-01-17", "2015-01-18", true);
            DateIsBefore("2014-12-31", "2015-01-01", true);
            DateIsBefore("2015-01-01", "2015-02-01", true);

            DateIsBefore("2015-01-01", "2015-01-01", false);
            DateIsBefore("2015-01-18", "2015-01-18", false);

            DateIsBefore("2015-01-18", "2015-01-17", false);
            DateIsBefore("2015-01-01", "2014-12-31", false);
            DateIsBefore("2015-02-01", "2015-01-01", false);
        }

        private void DateIsBefore(string beforeDateString, string afterDateString, bool expected)
        {
            var beforeDate = new Date(beforeDateString);
            var afterDate = new Date(afterDateString);
            Assert.That(beforeDate.IsBefore(afterDate), Is.EqualTo(expected));
        }
    }
}
