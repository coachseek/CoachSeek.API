namespace CoachSeek.Common
{
    public static class ErrorCodes
    {
        public static string DataMissing { get { return "data-missing"; } }

        public static string UserDuplicate { get { return "user-duplicate"; } }

        public static string CurrencyNotSupported { get { return "currency-not-supported"; } }
        public static string MerchantAccountIdentifierFormatInvalid { get { return "merchantAccountIdentifier-format-invalid"; } }
        public static string PaymentProviderRequiredWhenOnlineBookingIsEnabled { get { return "paymentprovider-required-when-online-booking-is-enabled"; } }
        public static string PaymentProviderNotSupported { get { return "paymentprovider-not-supported"; } }
        
        public static string LocationInvalid { get { return "location-invalid"; } }
        public static string LocationDuplicate { get { return "location-duplicate"; } }

        public static string DailyWorkingHoursInvalid { get { return "dailyworkinghours-invalid"; } }
        public static string CoachInvalid { get { return "coach-invalid"; } }
        public static string CoachDuplicate { get { return "coach-duplicate"; } }

        public static string ColourInvalid { get { return "colour-invalid"; } }

        public static string ServiceInvalid { get { return "service-invalid"; } }
        public static string ServiceDuplicate { get { return "service-duplicate"; } }

        public static string SessionChangeToCourseNotSupported { get { return "session-change-to-course-not-supported"; } }
        public static string SessionClashing { get { return "session-clashing"; } }
        public static string SessionFullyBooked { get { return "session-fully-booked"; } }
        public static string StandaloneSessionsMustBeBookedOneAtATime { get { return "standalone-session-must-be-booked-one-at-a-time"; } }

        public static string UseExistingCustomerForOnlineBookingNotSupported { get { return "existing-customer-for-online-booking-not-supported"; } }
        public static string CustomerInvalid { get { return "customer-invalid"; } }
        public static string CustomerDuplicate { get { return "customer-duplicate"; } }

        public static string BookingSessionRequired { get { return "booking-session-required"; } }
        public static string BookingUpdateNotSupported { get { return "booking-update-not-supported"; } }

        public static string PaymentStatusInvalid { get { return "payment-status-invalid"; } }

        public static string EmailTemplateTypeInvalid { get { return "email-template-type-invalid"; } }
    }
}