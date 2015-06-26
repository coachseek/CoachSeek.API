using System;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class CurrencyTests
    {
        [Test]
        public void GivenNullCurrencyCode_WhenConstructCurrency_ThenReturnDefaultCurrency()
        {
            var code = GivenNullCurrency();
            var currency = WhenConstructCurrency(code);
            ThenReturnDefaultCurrency(currency);
        }

        [Test]
        public void GivenEmptyCurrencyCode_WhenConstructCurrency_ThenReturnDefaultCurrency()
        {
            var code = GivenEmptyCurrency();
            var currency = WhenConstructCurrency(code);
            ThenReturnDefaultCurrency(currency);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(CurrencyNotSupported))]
        public void GivenIsNotFoundCurrency_WhenConstruct_ThenThrowCurrencyNotSupport()
        {
            var isNotFoundCurrency = GivenIsNotFoundCurrency();
            WhenConstructCurrency(isNotFoundCurrency);
        }

        [Test]
        public void GivenDefaultCurrencyCode_WhenConstructCurrency_ThenReturnDefaultCurrency()
        {
            var code = GivenDefaultCurrencyCode();
            var currency = WhenConstructCurrency(code);
            ThenReturnDefaultCurrency(currency);
        }

        public string GivenDefaultCurrencyCode()
        {
            return "NZD";
        }

        public string GivenNullCurrency()
        {
            return null;
        }

        public string GivenEmptyCurrency()
        {
            return "";
        }

        public string GivenIsNotFoundCurrency()
        {
            return "IsNotFoundCurrency";
        }
    

        public Currency WhenConstructCurrency(string currency)
        {
            return new Currency(currency, new HardCodedSupportedCurrencyRepository());   
        }


        public void ThenReturnDefaultCurrency(Currency currency)
        {
            Assert.That(currency.Code, Is.EqualTo("NZD"));
            Assert.That(currency.Symbol, Is.EqualTo("$"));
        }
    }
}
