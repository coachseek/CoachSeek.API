using System;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit
{
    public abstract class Tests
    {
        protected const int DAYS_IN_WEEK = 7;

        protected void AssertSingleError(object response, string expectedMessage, string expectedField = null)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;

            Assert.That(errors.Count, Is.EqualTo(1));

            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo(expectedMessage));
            Assert.That(error.Field, Is.EqualTo(expectedField));
        }

        protected void AssertMultipleErrors(object response, string[,] expectedErrors)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;

            Assert.That(errors.Count, Is.EqualTo(expectedErrors.GetLength(0)));

            var i = 0;
            foreach (var error in errors)
            {
                Assert.That(error.Message, Is.EqualTo(expectedErrors[i, 0]));
                Assert.That(error.Field, Is.EqualTo(expectedErrors[i, 1]));
                i++;
            }
        }

        protected string GetDateFormatNumberOfDaysOut(int dayCount, DateTime? today = null)
        {
            today = today ?? DateTime.Today;
            var oneWeekFromToday = today.Value.AddDays(dayCount);

            return oneWeekFromToday.ToString("yyyy-MM-dd");
        }

        protected string GetDateFormatOneWeekOut(DateTime? today = null)
        {
            today = today ?? DateTime.Today;
            var oneWeekFromToday = today.Value.AddDays(7);

            return oneWeekFromToday.ToString("yyyy-MM-dd");
        }
    }
}
