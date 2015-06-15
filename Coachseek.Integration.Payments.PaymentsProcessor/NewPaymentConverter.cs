using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public static class NewPaymentConverter
    {
        private const string PAYPAL_STATUS_COMPLETED = "Completed";
        private const string PAYPAL_STATUS_PENDING = "Pending";
        private const string PAYPAL_STATUS_DENIED = "Denied";


        public static NewPayment Convert(PaymentProcessingMessage message)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return ConvertFromPaypal(message.Contents);

            if (message.PaymentProvider == Constants.TEST)
                return CreateTestPayment(message);

            throw new InvalidOperationException("Unexpected payment provider.");
        }


        public static NewPayment ConvertFromPaypal(string paypalMessage)
        {
            var keyValuePairs = HttpUtility.ParseQueryString(paypalMessage);

            var id = keyValuePairs.Get("txn_id");
            var details = GetTransactionDetailsFromPaypal(keyValuePairs);
            var payer = GetPayerFromPaypal(keyValuePairs);
            var merchant = GetMerchantFromPaypal(keyValuePairs);
            var item = GetItemFromPaypal(keyValuePairs);

            return new NewPayment(id, details, payer, merchant, item, paypalMessage);
        }

        private static TransactionDetails GetTransactionDetailsFromPaypal(NameValueCollection keyValuePairs)
        {
            var status = GetTransactionStatusFromPaypal(keyValuePairs.Get("payment_status"));
            var date = GetTransactionDateFromPaypal(keyValuePairs.Get("payment_date"));
            var isTestMessage = GetIsTestingFromPaypal(keyValuePairs.Get("test_ipn"));

            return new TransactionDetails(status, PaymentProvider.PayPal, date, isTestMessage);
        }

        private static TransactionStatus GetTransactionStatusFromPaypal(string paypalPaymentStatus)
        {
            if (paypalPaymentStatus.CompareIgnoreCase(PAYPAL_STATUS_COMPLETED))
                return TransactionStatus.Completed;
            if (paypalPaymentStatus.CompareIgnoreCase(PAYPAL_STATUS_PENDING))
                return TransactionStatus.Pending;
            if (paypalPaymentStatus.CompareIgnoreCase(PAYPAL_STATUS_DENIED))
                return TransactionStatus.Denied;

            throw new InvalidOperationException("Unexpected PayPal payment status.");
        }

        private static DateTime GetTransactionDateFromPaypal(string paypalPaymentDate)
        {
            DateTime paymentDate;
            DateTime.TryParseExact(paypalPaymentDate,"HH:mm:ss MMM dd, yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out paymentDate);

            return paymentDate;
        }

        private static bool GetIsTestingFromPaypal(string paypalIsTesting)
        {
            return paypalIsTesting != null && paypalIsTesting == "1";
        }

        private static Payer GetPayerFromPaypal(NameValueCollection keyValuePairs)
        {
            var payerFirstName = keyValuePairs.Get("first_name");
            var payerLastName = keyValuePairs.Get("last_name");
            var payerEmail = keyValuePairs.Get("payer_email");

            return new Payer(payerFirstName, payerLastName, payerEmail);
        }

        private static Merchant GetMerchantFromPaypal(NameValueCollection keyValuePairs)
        {
            var businessId = keyValuePairs.Get("receiver_id");
            var businessName = keyValuePairs.Get("business");
            var businessEmail = keyValuePairs.Get("receiver_email");

            return new Merchant(businessId, businessName, businessEmail);
        }

        private static GoodOrService GetItemFromPaypal(NameValueCollection keyValuePairs)
        {
            var itemId = keyValuePairs.Get("item_number");
            var itemName = keyValuePairs.Get("item_name");
            var currency = keyValuePairs.Get("mc_currency");
            var grossAmount = keyValuePairs.Get("mc_gross").Parse<decimal>();

            return new GoodOrService(itemId, itemName, new Money(currency, grossAmount));
        }

        private static NewPayment CreateTestPayment(PaymentProcessingMessage testMessage)
        {
            return new NewPayment(testMessage.Id,
                                  null,
                                  null,
                                  null,
                                  null, 
                                  testMessage.Contents);
        }
    }
}
