using CoachSeek.Domain.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.WebUI.Tests.Unit.Exceptions
{
    [TestFixture]
    public class ValidationExceptionTests
    {
        [Test]
        public void GivenNoErrors_WhenCallErrorsProperty_ThenReturnZeroErrors()
        {
            var errors = GivenNoErrors();
            var errorsResponse = WhenCallErrorsProperty(errors);
            ThenReturnZeroErrors(errorsResponse);
        }

        [Test]
        public void GivenOneError_WhenCallErrorsProperty_ThenReturnOneError()
        {
            var errors = GivenOneError();
            var errorsResponse = WhenCallErrorsProperty(errors);
            ThenReturnOneError(errorsResponse);
        }

        [Test]
        public void GivenTwoErrors_WhenCallErrorsProperty_ThenReturnTwoErrors()
        {
            var errors = GivenTwoErrors();
            var errorsResponse = WhenCallErrorsProperty(errors);
            ThenReturnTwoErrors(errorsResponse);
        }


        private IEnumerable<Error> GivenNoErrors()
        {
            return new List<Error>();
        }

        private IEnumerable<Error> GivenOneError()
        {
            return new List<Error> { new Error(19, "A big error has occurred!") };
        }

        private IEnumerable<Error> GivenTwoErrors()
        {
            return new List<Error>
            {
                new Error(19, "A big error has occurred!"),
                new Error(23, "A smaller error has occurred.")
            };
        }

        private IList<Error> WhenCallErrorsProperty(IEnumerable<Error> errors)
        {
            var validationException = new ValidationException(errors);

            return validationException.Errors;
        }

        private void ThenReturnZeroErrors(IList<Error> errors)
        {
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Any(), Is.False);
        }

        private void ThenReturnOneError(IList<Error> errors)
        {
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Any(), Is.True);
            Assert.That(errors.Count, Is.EqualTo(1));
        }

        private void ThenReturnTwoErrors(IList<Error> errors)
        {
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Any(), Is.True);
            Assert.That(errors.Count, Is.EqualTo(2));
        }
    }
}
