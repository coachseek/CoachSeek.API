using System;
using System.Web;
using CoachSeek.Common;
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

            var transactionId = keyValuePairs.Get("txn_id");
            var status = GetTransactionStatus(keyValuePairs.Get("payment_status"));
            var isTestMessage = GetIsTesting(keyValuePairs.Get("test_ipn"));

            return new Payment(transactionId, status, isTestMessage);
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
    }
}
