namespace CoachSeek.Common
{
    public static class ErrorCodes
    {
        public static string UserDuplicate { get { return "user-duplicate"; } }

        public static string CurrencyNotSupported { get { return "currency-not-supported"; } }
        public static string PaymentProviderNotSupported { get { return "payment-provider-not-supported"; } }
        
        
        public static string LocationInvalid { get { return "location-invalid"; } }
        public static string LocationDuplicate { get { return "location-duplicate"; } }

        public static string CoachInvalid { get { return "coach-invalid"; } }
        public static string CoachDuplicate { get { return "coach-duplicate"; } }

        public static string ColourInvalid { get { return "colour-invalid"; } }

        public static string ServiceInvalid { get { return "service-invalid"; } }
        public static string ServiceDuplicate { get { return "service-duplicate"; } }
        
        public static string SessionClashing { get { return "session-clashing"; } }

        public static string CustomerInvalid { get { return "customer-invalid"; } }
        public static string CustomerDuplicate { get { return "customer-duplicate"; } }
    }
}