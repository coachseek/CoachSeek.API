using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class CurrencyTests
    {
        [Test]
        public void CurrencyCreationTests()
        {
            CreateCurrencySuccess(null, "NZD", "$");
            CreateCurrencySuccess("", "NZD", "$");
            CreateCurrencySuccess("NZD", "NZD", "$");
            CreateCurrencySuccess("AUD", "AUD", "$");
            CreateCurrencySuccess("USD", "USD", "$");
            CreateCurrencySuccess("SEK", "SEK", "kr");
            CreateCurrencySuccess("EUR", "EUR", "€");
            CreateCurrencySuccess("ZAR", "ZAR", "R");
            CreateCurrencySuccess("CNY", "CNY", "¥");

            CreateCurrencyError("XXX");
            CreateCurrencyError("IsNotFoundCurrency");
        }

        
        private void CreateCurrencySuccess(string currencyCode, string expectedCode, string expectedSymbol)
        {
            var currency = new Currency(currencyCode, new HardCodedSupportedCurrencyRepository());

            Assert.That(currency.Code, Is.EqualTo(expectedCode));
            Assert.That(currency.Symbol, Is.EqualTo(expectedSymbol));
        }

        private void CreateCurrencyError(string currencyCode)
        {
            try
            {
                var currency = new Currency(currencyCode, new HardCodedSupportedCurrencyRepository());
                Assert.Fail();
            }
            catch (CurrencyNotSupported)
            {
                // Pass test. Do nothing
            }
        }
    }
}
