using Coachseek.Integration.Payments;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Payments
{
    [TestFixture]
    public class PaypalPaymentsProviderApiTests
    {
        [Test]
        public void GivenMessageIsTestMessage_WhenGetPaypalUrl_ThenReturnsPaypalSandboxUrl()
        {
            var isTestMessage = GivenMessageIsTestMessage();
            var url = WhenGetUrl(isTestMessage);
            ThenReturnsPaypalSandboxUrl(url);
        }

        [Test]
        public void GivenMessageIsNotTestMessage_WhenGetPaypalUrl_ThenReturnsPaypalLiveUrl()
        {
            var isTestMessage = GivenMessageIsNotTestMessage();
            var url = WhenGetUrl(isTestMessage);
            ThenReturnsPaypalLiveUrl(url);
        }

        private bool GivenMessageIsTestMessage()
        {
            return true;
        }

        private bool GivenMessageIsNotTestMessage()
        {
            return false;
        }


        private string WhenGetUrl(bool isTestMessage)
        {
            var provider = new PaypalPaymentProviderApi(isTestMessage);
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
