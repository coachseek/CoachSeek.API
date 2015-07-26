using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Exceptions;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public static class NewPaymentConverter
    {
        private const string PAYPAL_STATUS_COMPLETED = "Completed";
        private const string PAYPAL_STATUS_PENDING = "Pending";
        private const string PAYPAL_STATUS_DENIED = "Denied";


        public static NewPayment Convert(PaymentProcessingMessage message)
        {
            try
            {
                return ConvertMessage(message);
            }
            catch (ValidationException ex)
            {
                throw new PaymentConversionException(ex);
            }
        }


        private static NewPayment ConvertMessage(PaymentProcessingMessage message)
        {
            if (message.PaymentProvider == Constants.PAYPAL)
                return ConvertFromPaypal(message.Contents);

            if (message.PaymentProvider == Constants.TEST)
                return ConvertFromTestMessage(message);

            throw new UnexpectedPaymentProvider(message.PaymentProvider);
        }

        public static NewPayment ConvertFromPaypal(string paypalMessage)
        {
            var keyValuePairs = HttpUtility.ParseQueryString(paypalMessage);

            var validation = new ValidationException();

            var id = keyValuePairs.Get("txn_id");
            var details = GetTransactionDetailsFromPaypal(keyValuePairs);
            var payer = GetPayerFromPaypal(keyValuePairs);
            var merchant = GetMerchantFromPaypal(keyValuePairs, validation);
            var item = GetItemFromPaypal(keyValuePairs, validation);

            validation.ThrowIfErrors();

            return new NewPayment(id, details, payer, merchant, item, paypalMessage);
        }

        private static TransactionDetails GetTransactionDetailsFromPaypal(NameValueCollection keyValuePairs)
        {
            var status = GetTransactionStatusFromPaypal(keyValuePairs.Get("payment_status"));
            var date = GetTransactionDateFromPaypal(keyValuePairs.Get("payment_date"));
            var isTestMessage = GetIsTestingFromPaypal(keyValuePairs.Get("test_ipn"));

            return new NewTransactionDetails(status, PaymentProvider.PayPal, date, isTestMessage);
        }

        private static TransactionStatus GetTransactionStatusFromPaypal(string paypalPaymentStatus)
        {
            if (paypalPaymentStatus.CompareIgnoreCase(PAYPAL_STATUS_COMPLETED))
                return TransactionStatus.Completed;
            if (paypalPaymentStatus.CompareIgnoreCase(PAYPAL_STATUS_PENDING))
                return TransactionStatus.Pending;
            if (paypalPaymentStatus.CompareIgnoreCase(PAYPAL_STATUS_DENIED))
                return TransactionStatus.Denied;

            throw new UnexpectedPaymentStatus(Constants.PAYPAL, paypalPaymentStatus);
        }

        private static DateTime GetTransactionDateFromPaypal(string paypalPaymentDate)
        {
            paypalPaymentDate = paypalPaymentDate.Substring(0, paypalPaymentDate.Length - 4);
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

        private static Merchant GetMerchantFromPaypal(NameValueCollection keyValuePairs, ValidationException errors)
        {
            var businessId = GetBusinessIdFromPaypal(keyValuePairs, errors);
            var businessName = keyValuePairs.Get("business") ?? keyValuePairs.Get("receiver_email");
            var businessEmail = keyValuePairs.Get("receiver_email") ?? keyValuePairs.Get("business");

            return new Merchant(businessId, businessName, businessEmail);
        }

        private static GoodOrService GetItemFromPaypal(NameValueCollection keyValuePairs, ValidationException errors)
        {
            var itemId = GetItemIdFromPaypal(keyValuePairs, errors);
            var itemName = keyValuePairs.Get("item_name");
            var currency = keyValuePairs.Get("mc_currency");
            var grossAmount = keyValuePairs.Get("mc_gross").Parse<decimal>();

            return new GoodOrService(itemId, itemName, new Money(currency, grossAmount));
        }

        private static Guid GetBusinessIdFromPaypal(NameValueCollection keyValuePairs, ValidationException errors)
        {
            try
            {
                var businessId = keyValuePairs.Get("custom");
                return new Guid(businessId);
            }
            catch (Exception)
            {
                errors.Add("The BusinessId must be a GUID.");
                return Guid.Empty;
            }
        }

        private static Guid GetItemIdFromPaypal(NameValueCollection keyValuePairs, ValidationException errors)
        {
            try
            {
                var itemId = keyValuePairs.Get("item_number");
                return new Guid(itemId);
            }
            catch (Exception)
            {
                errors.Add("The ItemId must be a GUID.");
                return Guid.Empty;
            }
        }

        private static NewPayment ConvertFromTestMessage(PaymentProcessingMessage testMessage)
        {
            var keyValuePairs = HttpUtility.ParseQueryString(testMessage.Contents);

            return new NewPayment(testMessage.Id,
                                  GetTransactionDetailsFromTest(keyValuePairs),
                                  GetPayerFromTest(keyValuePairs),
                                  GetMerchantFromTest(keyValuePairs),
                                  GetItemFromTest(keyValuePairs), 
                                  testMessage.Contents);
        }

        private static TransactionDetails GetTransactionDetailsFromTest(NameValueCollection keyValuePairs)
        {
            var status = keyValuePairs.Get("status").Parse<TransactionStatus>();
            var date = keyValuePairs.Get("date").Parse<DateTime>();
            var isTesting = keyValuePairs.Get("isTesting").Parse<bool>();

            return new NewTransactionDetails(status, PaymentProvider.Test, date, isTesting);
        }

        private static Payer GetPayerFromTest(NameValueCollection keyValuePairs)
        {
            var payerFirstName = keyValuePairs.Get("payerFirstName");
            var payerLastName = keyValuePairs.Get("payerLastName");
            var payerEmail = keyValuePairs.Get("payerEmail");

            return new Payer(payerFirstName, payerLastName, payerEmail);
        }

        private static Merchant GetMerchantFromTest(NameValueCollection keyValuePairs)
        {
            var businessId = keyValuePairs.Get("businessId").Parse<Guid>();
            var businessName = keyValuePairs.Get("businessName");
            var businessEmail = keyValuePairs.Get("businessEmail");

            return new Merchant(businessId, businessName, businessEmail);
        }

        private static GoodOrService GetItemFromTest(NameValueCollection keyValuePairs)
        {
            var itemId = keyValuePairs.Get("itemId").Parse<Guid>();
            var itemName = keyValuePairs.Get("itemName");
            var currency = keyValuePairs.Get("currency");
            var grossAmount = keyValuePairs.Get("grossAmount").Parse<decimal>();

            return new GoodOrService(itemId, itemName, new Money(currency, grossAmount));
        }
    }
}
