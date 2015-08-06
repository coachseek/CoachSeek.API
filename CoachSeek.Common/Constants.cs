namespace CoachSeek.Common
{
    public static class Constants
    {
        public const string BUSINESS_DOMAIN = "Business-Domain";
        public const string AUTHORIZATION = "Authorization";

        public const string ANONYMOUS_USER = "anonymous";

        // Payment Providers
        public const string PAYPAL = "PayPal";
        public const string TEST = "Test";

        public const string TRANSACTION_PAYMENT = "Payment";

        // Payment Status
        public const string PAYMENT_STATUS_PENDING_INVOICE = "pending-invoice";
        public const string PAYMENT_STATUS_PENDING_PAYMENT = "pending-payment";
        public const string PAYMENT_STATUS_OVERDUE_PAYMENT = "overdue-payment";
        public const string PAYMENT_STATUS_PAID = "paid";

        // Email Templates
        public const string EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING = "CustomerSessionBooking";
        public const string EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING = "CustomerCourseBooking";
    }
}
