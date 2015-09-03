using System;
using CoachSeek.Common;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Payments.PaymentsProcessor;
using NUnit.Framework;

namespace Coachseek.Integration.Tests.Unit.Payments.Tests
{
    [TestFixture]
    public class NewPaymentConverterTests
    {
        [Test]
        public void ConvertPaypalMessage()
        {
            var paypalMessage = PaymentProcessingMessage.Create("PayPal", "mc_gross=253.00&protection_eligibility=Ineligible&address_status=confirmed&payer_id=VBAWNJWEZW5MU&tax=0.00&address_street=1+Main+St&payment_date=10%3A52%3A17+Jun+16%2C+2015+PDT&payment_status=Pending&charset=windows-1252&address_zip=95131&first_name=test&address_country_code=US&address_name=test+buyer&notify_version=3.8&custom=403CF70F-77A0-45F4-A5ED-3AC74949BF0C&payer_status=verified&business=readelobdill-facilitator%40aim.com&address_country=United+States&address_city=San+Jose&quantity=1&verify_sign=A--8MSCLabuvN8L.-MHjxC9uypBtAuTW3aj9R79RoLkytkmqCson8RI2&payer_email=readelobdill-buyer%40aim.com&txn_id=16E733388V669700M&payment_type=instant&last_name=buyer&address_state=CA&receiver_email=readelobdill-facilitator%40aim.com&receiver_id=9J5KEFUJRX3AS&pending_reason=multi_currency&txn_type=web_accept&item_name=15min+Session&mc_currency=GBP&item_number=3F1ECB64-C368-4619-A511-59BA3BE4D92E&residence_country=US&test_ipn=1&handling_amount=0.00&transaction_subject=&payment_gross=&shipping=0.00&ipn_track_id=a2ab398648288");
            var newPayment = NewPaymentConverter.Convert(paypalMessage);
            AssertNewPaypalPayment(newPayment);
        }

        private void AssertNewPaypalPayment(NewPayment newPayment)
        {
            Assert.That(newPayment.Id, Is.EqualTo("16E733388V669700M"));
            Assert.That(newPayment.PaymentProvider, Is.EqualTo("PayPal"));
            Assert.That(newPayment.Type, Is.EqualTo("Payment"));
            Assert.That(newPayment.TransactionStatus, Is.EqualTo(TransactionStatus.Pending));
            Assert.That(newPayment.TransactionDate, Is.EqualTo(new DateTime(2015, 6, 16, 10, 52, 17)));
            Assert.That(newPayment.IsTesting, Is.True);
            Assert.That(newPayment.IsPending, Is.True);
            Assert.That(newPayment.IsCompleted, Is.False);
            Assert.That(newPayment.IsDenied, Is.False);
            Assert.That(newPayment.ItemId, Is.EqualTo(new Guid("3F1ECB64-C368-4619-A511-59BA3BE4D92E")));
            Assert.That(newPayment.ItemName, Is.EqualTo("15min Session"));
            Assert.That(newPayment.ItemCurrency, Is.EqualTo("GBP"));
            Assert.That(newPayment.ItemAmount, Is.EqualTo(253));
            Assert.That(newPayment.MerchantId, Is.EqualTo(new Guid("403CF70F-77A0-45F4-A5ED-3AC74949BF0C")));
            Assert.That(newPayment.MerchantName, Is.EqualTo("readelobdill-facilitator@aim.com"));
            Assert.That(newPayment.MerchantEmail, Is.EqualTo("readelobdill-facilitator@aim.com"));
            Assert.That(newPayment.PayerFirstName, Is.EqualTo("test"));
            Assert.That(newPayment.PayerLastName, Is.EqualTo("buyer"));
            Assert.That(newPayment.PayerEmail, Is.EqualTo("readelobdill-buyer@aim.com"));
        }
    }
}
