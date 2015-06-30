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

        [Test]
        public void GivenCorrectCurrencyCode_WhenConstructCurrency_ThenReturnCorrectCurrency()
        {
            var code_GBP = GivenCorrectCurrencyCode("GBP","£");
            var currency_GBP = WhenConstructCurrency(code_GBP[0]);
            ThenReturnCorrectCurrency(currency_GBP,code_GBP[0], code_GBP[1]);

            var code_EUR = GivenCorrectCurrencyCode("EUR", "€");
            var currency_EUR = WhenConstructCurrency(code_EUR[0]);
            ThenReturnCorrectCurrency(currency_EUR, code_EUR[0],code_EUR[1]);

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

        public string[] GivenCorrectCurrencyCode(string code,string symbol)
        {
            return new[]{code,symbol};
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

        public void ThenReturnCorrectCurrency(Currency currency,string code,string symbol)
        {
            Assert.That(currency.Code,Is.EqualTo(code));
            Assert.That(currency.Symbol, Is.EqualTo(symbol));
        }
    }
}
