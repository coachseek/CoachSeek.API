using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Controllers;
using NUnit.Framework;

namespace CoachSeek.WebUI.Tests.Unit.Controllers
{
    [TestFixture]
    public class PaypalControllerTests
    {
        private PaypalController Controller { get; set; }


        [SetUp]
        public void Setup()
        {
            Controller = new PaypalController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }


        [Test]
        public void GivenIsPaymentEnabledIsFalse_WhenGetPaypalUrl_ThenReturnsPaypalSandboxUrl()
        {
            GivenIsPaymentEnabledIsFalse();
            var paypalUrl = WhenGetPaypalUrl();
            ThenReturnsPaypalSandboxUrl(paypalUrl);
        }

        [Test]
        public void GivenIsPaymentEnabledIsTrue_WhenGetPaypalUrl_ThenReturnsPaypalLiveUrl()
        {
            GivenIsPaymentEnabledIsTrue();
            var paypalUrl = WhenGetPaypalUrl();
            ThenReturnsPaypalLiveUrl(paypalUrl);
        }


        private void GivenIsPaymentEnabledIsFalse()
        {
            Controller.IsPaymentEnabled = false;
        }

        private void GivenIsPaymentEnabledIsTrue()
        {
            Controller.IsPaymentEnabled = true;
        }


        private string WhenGetPaypalUrl()
        {
            return Controller.PaypalUrl;
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
