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

        public static string DurationInvalid { get { return "duration-invalid"; } }
        public static string ColourInvalid { get { return "colour-invalid"; } }
        public static string StudentCapacityInvalid { get { return "studentcapacity-invalid"; } }
        public static string SessionCountInvalid { get { return "sessioncount-invalid"; } }
        public static string RepeatFrequencyInvalid { get { return "repeatfrequency-invalid"; } }
        public static string PriceInvalid { get { return "price-invalid"; } }
        public static string SessionPriceInvalid { get { return "sessionprice-invalid"; } }
        public static string CoursePriceInvalid { get { return "courseprice-invalid"; } }

        public static string ServiceInvalid { get { return "service-invalid"; } }
        public static string ServiceDuplicate { get { return "service-duplicate"; } }
        public static string ServiceIsPricedButHasNoPrices { get { return "service-is-priced-but-has-no-prices"; } }
        public static string ServiceForStandaloneSessionMustHaveNoCoursePrice { get { return "service-for-standalone-session-must-have-no-courseprice"; } }

        public static string SessionChangeToCourseNotSupported { get { return "session-change-to-course-not-supported"; } }
        public static string SessionClashing { get { return "session-clashing"; } }
        public static string SessionNotInCourse { get { return "session-not-in-course"; } }
        public static string SessionFullyBooked { get { return "session-fully-booked"; } }
        public static string StandaloneSessionMustBeBookedOneAtATime { get { return "standalone-session-must-be-booked-one-at-a-time"; } }
        public static string StandaloneSessionMustHaveNoRepeatFrequency { get { return "standalone-session-must-have-no-repeatfrequency"; } }
        public static string StandaloneSessionMustHaveNoCoursePrice { get { return "standalone-session-must-have-no-courseprice"; } }
        public static string StandaloneSessionMustHaveSessionPrice { get { return "standalone-session-must-have-sessionprice"; } }
        public static string CourseMustHaveRepeatFrequency { get { return "course-must-have-repeatfrequency"; } }
        public static string CourseExceedsMaximumNumberOfDailySessions { get { return "course-exceeds-maximum-number-of-daily-sessions"; } }
        public static string CourseExceedsMaximumNumberOfWeeklySessions { get { return "course-exceeds-maximum-number-of-weekly-sessions"; } }

        public static string UseExistingCustomerForOnlineBookingNotSupported { get { return "existing-customer-for-online-booking-not-supported"; } }
        public static string CustomerInvalid { get { return "customer-invalid"; } }
        public static string CustomerDuplicate { get { return "customer-duplicate"; } }
        public static string CustomerAlreadyBookedOntoSession { get { return "customer-already-booked-onto-session"; } }

        public static string BookingSessionRequired { get { return "booking-session-required"; } }
        public static string BookingUpdateNotSupported { get { return "booking-update-not-supported"; } }

        public static string PaymentStatusInvalid { get { return "paymentstatus-invalid"; } }

        public static string EmailTemplateTypeInvalid { get { return "emailtemplate-type-invalid"; } }
    }
}