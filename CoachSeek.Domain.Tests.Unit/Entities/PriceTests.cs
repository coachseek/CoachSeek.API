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
        public void PriceCreationTests()
        {
            PriceCreationFailure(-1.2m);        // No negative prices.
            PriceCreationFailure(123.456m);     // 2 decimal places max.

            PriceCreationSuccess(null);
            PriceCreationSuccess(0);
            PriceCreationSuccess(62);
            PriceCreationSuccess(456.78m);      // 2 decimal places max.
        }

        [Test]
        public void ItemQuantityCreationTests()
        {
            PriceCreationFailure(-8.6m, -5);     // No negatives.
            PriceCreationFailure(-7.8m, 35);     // No negatives.
            PriceCreationFailure(4.35m, -9);     // No negatives.
            PriceCreationFailure(123.456m, -3);  // 2 decimal places max., no negatives

            PriceCreationSuccess(0, 17);
            PriceCreationSuccess(6.2m, 0);
            PriceCreationSuccess(5.8m, 15);
            PriceCreationSuccess(2.35m, 15);    // 2 decimal places max.
        }


        private void PriceCreationSuccess(decimal? inputAmount)
        {
            var price = new Price(inputAmount);
            Assert.That(price, Is.Not.Null);
            Assert.That(price.Amount, Is.EqualTo(inputAmount));
        }

        private void PriceCreationSuccess(decimal itemAmount, int quantity)
        {
            var price = new Price(itemAmount, quantity);
            Assert.That(price, Is.Not.Null);
            Assert.That(price.Amount, Is.EqualTo(itemAmount * quantity));
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

        private void PriceCreationFailure(decimal itemAmount, int quantity)
        {
            try
            {
                var price = new Price(itemAmount, quantity);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidPrice>());
            }
        }
    }
}
