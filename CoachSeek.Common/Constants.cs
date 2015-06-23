﻿namespace CoachSeek.Common
{
    public static class Constants
    {
        public const string ANONYMOUS_USER = "anonymous";

        // Payment Providers
        public const string PAYPAL = "PayPal";
        public const string TEST = "Test";

        public const string TRANSACTION_PAYMENT = "Payment";

        // Payment Status
        public const string PAYMENT_STATUS_PENDING_INVOICE = "pending-invoice";
        public const string PAYMENT_STATUS_PENDING_PAYMENT = "pending-payment";
        public const string PAYMENT_STATUS_PAID = "paid";
    }
}
