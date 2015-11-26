using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class CourseBookingPaymentStatusCalculatorTests
    {
        [Test]
        public void PaymentStatusTests()
        {
            AssertPaymentStatus("overdue-payment", "overdue-payment");
            AssertPaymentStatus("pending-invoice", "pending-invoice");
            AssertPaymentStatus("pending-payment", "pending-payment");
            AssertPaymentStatus("paid", "paid");

            AssertPaymentStatus("overdue-payment", "paid", "overdue-payment", "pending-invoice", "pending-payment");
            AssertPaymentStatus("pending-invoice", "paid", "pending-payment", "pending-invoice", "pending-payment");
            AssertPaymentStatus("pending-payment", "paid", "pending-payment", "paid", "pending-payment");
            AssertPaymentStatus("paid", "paid", "paid", "paid");
        }

        private void AssertPaymentStatus(string expected, params string[] paymentStatii)
        {
            var sessionBookings = paymentStatii.Select(paymentStatus => new SingleSessionBookingData {PaymentStatus = paymentStatus}).ToList();
            var status = CourseBookingPaymentStatusCalculator.CalculatePaymentStatus(sessionBookings);
            Assert.That(status, Is.EqualTo(expected));
        }
    }
}
