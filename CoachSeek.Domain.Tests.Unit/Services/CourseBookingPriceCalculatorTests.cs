using System;
using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class CourseBookingPriceCalculatorTests
    {
        [Test]
        public void CourseBookingPriceTests()
        {
            AssertPriceCalculationFailure(0, 10, null, null);
            AssertPriceCalculationFailure(10, 10, null, null);

            AssertPriceCalculationSuccess(0, 10, 180, 20, 0);
            AssertPriceCalculationSuccess(5, 10, 180, 20, 100);
            AssertPriceCalculationSuccess(10, 10, 180, 20, 180);
            AssertPriceCalculationSuccess(10, 10, 200, 20, 200);
            AssertPriceCalculationSuccess(10, 10, 250, 20, 250);

            AssertPriceCalculationSuccess(0, 7, 100, null, 0);
            AssertPriceCalculationSuccess(3, 7, 100, null, 42.87m);
            AssertPriceCalculationSuccess(6, 7, 100, null, 85.74m);
            AssertPriceCalculationSuccess(7, 7, 100, null, 100);

            AssertPriceCalculationSuccess(0, 5, null, 20, 0);
            AssertPriceCalculationSuccess(2, 5, null, 20, 40);
            AssertPriceCalculationSuccess(5, 5, null, 20, 100);
            AssertPriceCalculationSuccess(3, 5, null, 13.67m, 41.01m);
        }


        private void AssertPriceCalculationFailure(int numberOfSessionsInBooking,
                                                   int numberOfSessionsInCourse,
                                                   decimal? coursePrice,
                                                   decimal? sessionPrice)
        {
            try
            {
                CourseBookingPriceCalculator.CalculatePrice(numberOfSessionsInBooking, 
                                                            numberOfSessionsInCourse, 
                                                            coursePrice, 
                                                            sessionPrice);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        private void AssertPriceCalculationSuccess(int numberOfSessionsInBooking,
                                                   int numberOfSessionsInCourse,
                                                   decimal? coursePrice,
                                                   decimal? sessionPrice,
                                                   decimal expectedBookingPrice)
        {
            var price = CourseBookingPriceCalculator.CalculatePrice(numberOfSessionsInBooking,
                                                                    numberOfSessionsInCourse,
                                                                    coursePrice,
                                                                    sessionPrice);
            Assert.That(price, Is.EqualTo(expectedBookingPrice));
        }
    }
}
