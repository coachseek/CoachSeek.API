using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class PriceTests
    {
        [Test]
        public void DurationCreationTests()
        {
            PriceCreationFailure(-1.2m); // No negative prices.
            PriceCreationFailure(123.456m); // 2 decimal places max.

            PriceCreationSuccess(null);
            PriceCreationSuccess(0);
            PriceCreationSuccess(62);
            PriceCreationSuccess(456.78m); // 2 decimal places max.
        }


        private void PriceCreationSuccess(decimal? inputAmount)
        {
            var price = new Price(inputAmount);
            Assert.That(price, Is.Not.Null);
            Assert.That(price.Amount, Is.EqualTo(inputAmount));
        }

        private void PriceCreationFailure(decimal? inputAmount)
        {
            try
            {
                var price = new Price(inputAmount);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidPrice>());
            }
        }
    }
}
