using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit
{
    public abstract class Tests
    {
        protected void AssertSingleError(object response, string message, string field)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo(message));
            Assert.That(error.Field, Is.EqualTo(field));
        }
    }
}
