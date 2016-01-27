namespace CoachSeek.Common
{
    public static class Constants
    {
        public const string BUSINESS_DOMAIN = "Business-Domain";
        public const string AUTHORIZATION = "Authorization";

        public const string ANONYMOUS_USER = "anonymous";

        // Subscription Plans
        public const string SUBSCRIPTION_TRIAL = "Trial";
        public const string SUBSCRIPTION_SOLO = "Solo";
        public const string SUBSCRIPTION_TEAM = "Team";
        public const string SUBSCRIPTION_PRO = "Pro";
        public const string SUBSCRIPTION_UNLIMITED = "Unlimited";

        public const int MAXIMUM_NUMBER_OF_COACHES = 100;

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
        public const string EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION = "OnlineBookingCustomerSession";
        public const string EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_COURSE = "OnlineBookingCustomerCourse";

        // Custom Field Types
        public const string CUSTOM_FIELD_TYPE_CUSTOMER = "customer";
    }
}
