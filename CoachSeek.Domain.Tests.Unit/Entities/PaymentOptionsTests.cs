using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class PaymentOptionsTests
    {
        [Test]
        public void GivenIsOnlinePaymentEnabledTrueButNoPaymentProvider_WhenTryConstructPaymentOptions_ThenNoPaymentProviderError()
        {
            var parameters = GivenIsOnlinePaymentEnabledTrueButNoPaymentProvider();
            var response = WhenTryConstructPaymentOptions(parameters);
            ThenNoPaymentProviderError(response);
        }


        private Tuple<bool, bool, string, string> GivenIsOnlinePaymentEnabledTrueButNoPaymentProvider()
        {
            return new Tuple<bool, bool, string, string>(true, false, null, null);
        }

        private object WhenTryConstructPaymentOptions(Tuple<bool, bool, string, string> parameters)
        {
            try
            {
                return new PaymentOptions(parameters.Item1, parameters.Item2, parameters.Item3, parameters.Item4);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void ThenNoPaymentProviderError(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var validationException = (ValidationException) response;
            Assert.That(validationException.Errors.Count, Is.EqualTo(1));
            var error = validationException.Errors[0];
            Assert.That(error.Message, Is.EqualTo("When Online Payment is enabled then an Online Payment Provider must be specified."));
        }
    }
}
