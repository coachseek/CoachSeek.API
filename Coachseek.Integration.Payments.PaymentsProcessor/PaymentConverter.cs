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
    public static class PaymentConverter
    {
        public static Payment Convert(PaymentProcessingMessage message)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return ConvertFromPaypal(message.Contents);

            throw new InvalidOperationException("Unexpected payment provider.");
        }

        public static Payment ConvertFromPaypal(string paypalMessage)
        {
            var keyValuePairs = HttpUtility.ParseQueryString(paypalMessage);

            var details = GetTransactionDetails(keyValuePairs);
            var payer = GetPayer(keyValuePairs);
            var merchant = GetMerchant(keyValuePairs);
            var item = GetItem(keyValuePairs);

            return new Payment(details, payer, merchant, item);
        }

        private static TransactionDetails GetTransactionDetails(NameValueCollection keyValuePairs)
        {
            var id = keyValuePairs.Get("txn_id");
            var status = GetTransactionStatus(keyValuePairs.Get("payment_status"));
            var date = GetTransactionDate(keyValuePairs.Get("payment_date"));
            var isTestMessage = GetIsTesting(keyValuePairs.Get("test_ipn"));

            return new TransactionDetails(id, status, date, isTestMessage);
        }

        private static DateTime GetTransactionDate(string paypalPaymentDate)
        {
            var date = paypalPaymentDate.Substring(3).Trim();
            DateTime paymentDate;

            var success = DateTime.TryParseExact(paypalPaymentDate, 
                                   "MMM dd HH:mm:ss", 
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None, 
                                   out paymentDate);

            return paymentDate;
        }

        private static bool GetIsTesting(string paypalIsTesting)
        {
            return paypalIsTesting != null && paypalIsTesting == "1";
        }

        private static TransactionStatus GetTransactionStatus(string paypalPaymentStatus)
        {
            if (paypalPaymentStatus.ToLower() == "completed")
                return TransactionStatus.Completed;

            return TransactionStatus.Incomplete;
        }

        private static Payer GetPayer(NameValueCollection keyValuePairs)
        {
            var payerFirstName = keyValuePairs.Get("first_name");
            var payerLastName = keyValuePairs.Get("last_name");
            var payerEmail = keyValuePairs.Get("payer_email");

            return new Payer(payerFirstName, payerLastName, payerEmail);
        }

        private static Merchant GetMerchant(NameValueCollection keyValuePairs)
        {
            var businessId = keyValuePairs.Get("receiver_id");
            var businessName = keyValuePairs.Get("business");
            var businessEmail = keyValuePairs.Get("receiver_email");

            return new Merchant(businessId, businessName, businessEmail);
        }

        private static GoodOrService GetItem(NameValueCollection keyValuePairs)
        {
            var itemId = keyValuePairs.Get("item_number1");
            var itemName = keyValuePairs.Get("item_name1");
            var currency = keyValuePairs.Get("mc_currency");
            var grossAmount = keyValuePairs.Get("mc_gross1").Parse<decimal>();

            return new GoodOrService(itemId, itemName, new Money(currency, grossAmount));
        }
    }
}
