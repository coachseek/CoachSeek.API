using Coachseek.Integration.Payments;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Payments
{
    [TestFixture]
    public class PaypalPaymentsProviderApiTests
    {
        [Test]
        public void GivenIsPaymentEnabledIsFalse_WhenGetPaypalUrl_ThenReturnsPaypalSandboxUrl()
        {
            var isPaymentEnabled = GivenIsPaymentEnabledIsFalse();
            var url = WhenGetUrl(isPaymentEnabled);
            ThenReturnsPaypalSandboxUrl(url);
        }

        [Test]
        public void GivenIsPaymentEnabledIsTrue_WhenGetPaypalUrl_ThenReturnsPaypalLiveUrl()
        {
            var isPaymentEnabled = GivenIsPaymentEnabledIsTrue();
            var url = WhenGetUrl(isPaymentEnabled);
            ThenReturnsPaypalLiveUrl(url);
        }

        private bool GivenIsPaymentEnabledIsFalse()
        {
            return false;
        }

        private bool GivenIsPaymentEnabledIsTrue()
        {
            return true;
        }


        private string WhenGetUrl(bool isPaymentEnabled)
        {
            var provider = new PaypalPaymentsProviderApi(isPaymentEnabled);
            return provider.Url;
        }


        private void ThenReturnsPaypalSandboxUrl(string paypalUrl)
        {
            Assert.That(paypalUrl, Is.EqualTo("https://www.sandbox.paypal.com/cgi-bin/webscr"));
        }

        private void ThenReturnsPaypalLiveUrl(string paypalUrl)
        {
            Assert.That(paypalUrl, Is.EqualTo("https://www.paypal.com/cgi-bin/webscr"));
        }
    }
}
