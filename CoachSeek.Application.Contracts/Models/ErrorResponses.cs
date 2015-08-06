using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models
{
    public class NoDataErrorResponse : ErrorResponse
    {
        public NoDataErrorResponse()
            : base(Resources.ErrorNoData)
        { }
    }

    public class MissingBusinessRegistrationDataErrorResponse : ErrorResponse
    {
        public MissingBusinessRegistrationDataErrorResponse() 
            : base(Resources.ErrorNoBusinessAddData)
        { }
    }

    public class CurrencyNotSupportedErrorResponse : ErrorResponse
    {
        public CurrencyNotSupportedErrorResponse()
            : base(Resources.ErrorCurrencyNotSupported, "registration.business.currency")
        { }

        public CurrencyNotSupportedErrorResponse(string field)
            : base(Resources.ErrorCurrencyNotSupported, field)
        { }
    }

    public class DuplicateUserErrorResponse : ErrorResponse
    {
        public DuplicateUserErrorResponse()
            : base(Resources.ErrorUserDuplicateEmail, "registration.admin.email")
        { }
    }

    public class DuplicateLocationErrorResponse : ErrorResponse
    {
        public DuplicateLocationErrorResponse()
            : base(Resources.ErrorDuplicateLocation, "location.name")
        { }
    }

    public class DuplicateCoachErrorResponse : ErrorResponse
    {
        public DuplicateCoachErrorResponse()
            : base(Resources.ErrorDuplicateCoach)
        { }
    }

    public class DuplicateCustomerErrorResponse : ErrorResponse
    {
        public DuplicateCustomerErrorResponse()
            : base(Resources.ErrorDuplicateCustomer)
        { }
    }

    public class DuplicateServiceErrorResponse : ErrorResponse
    {
        public DuplicateServiceErrorResponse()
            : base(Resources.ErrorDuplicateService, "service.name")
        { }
    }

    public class InvalidLocationErrorResponse : ErrorResponse
    {
        public InvalidLocationErrorResponse()
            : base(Resources.ErrorInvalidLocation, "location.id")
        { }
    }

    public class InvalidCoachErrorResponse  : ErrorResponse
    {
        public InvalidCoachErrorResponse()
            : base(Resources.ErrorInvalidCoach, "coach.id")
        { }
    }
    public class InvalidServiceErrorResponse : ErrorResponse
    {
        public InvalidServiceErrorResponse()
            : base(Resources.ErrorInvalidService, "service.id")
        { }
    }

    public class InvalidSessionErrorResponse : ErrorResponse
    {
        public InvalidSessionErrorResponse()
            : base(Resources.ErrorInvalidSession)
        { }
    }

    public class SessionsInCourseBookingMustBelongToSameCourseErrorResponse : ErrorResponse
    {
        public SessionsInCourseBookingMustBelongToSameCourseErrorResponse()
            : base(Resources.ErrorSessionsInCourseBookingBelongToSameCourse)
        { }
    }

    public class InvalidCustomerErrorResponse : ErrorResponse
    {
        public InvalidCustomerErrorResponse()
            : base(Resources.ErrorInvalidCustomer, "customer.id")
        { }

        public InvalidCustomerErrorResponse(string field)
            : base(Resources.ErrorInvalidCustomer, field)
        { }
    }

    public class InvalidBookingErrorResponse : ErrorResponse
    {
        public InvalidBookingErrorResponse()
            : base(Resources.ErrorInvalidBooking, "booking.id")
        { }
    }

    public class InvalidEmailTemplateErrorResponse : ErrorResponse
    {
        public InvalidEmailTemplateErrorResponse()
            : base(Resources.ErrorInvalidPaymentStatus)
        { }
    }

    public class InvalidPaymentStatusErrorResponse : ErrorResponse
    {
        public InvalidPaymentStatusErrorResponse()
            : base(Resources.ErrorInvalidPaymentStatus)
        { }
    }

    public class ClashingSessionErrorResponse : ErrorResponse
    {
        public ClashingSessionErrorResponse(ClashingSession clashingSession)
            : base(Resources.ErrorClashingSession, null, ErrorCode.ClashingSession, FormatClashingSessionMessage(clashingSession))
        { }

        private static string FormatClashingSessionMessage(ClashingSession clashingSession)
        {
            return string.Format("Clashing session: {0}; SessionId = {{{1}}}", 
                clashingSession.Session.Name,
                clashingSession.Session.Id);
        }
    }

    public class InvalidEmailAddressFormatErrorResponse : ErrorResponse
    {
        public InvalidEmailAddressFormatErrorResponse(string field)
            : base(Resources.ErrorInvalidEmailAddressFormat, field)
        { }
    }

    // Temporary Error Responses
    public class CannotUpdateCourseErrorResponse : ErrorResponse
    {
        public CannotUpdateCourseErrorResponse()
            : base("Course updates are not working yet.")
        { }
    }

    public class CannotChangeSessionToCourseErrorResponse : ErrorResponse
    {
        public CannotChangeSessionToCourseErrorResponse()
            : base("Cannot change a session to a course.")
        { }
    }

    public class CannotChangeCourseRepetitionErrorResponse : ErrorResponse
    {
        public CannotChangeCourseRepetitionErrorResponse()
            : base("Cannot change the repetition of a course.")
        { }
    }

    //public class CannotChangeStandaloneSessionToCourseErrorResponse : ErrorResponse
    //{
    //    public CannotChangeStandaloneSessionToCourseErrorResponse()
    //        : base("Cannot change from a standalone session to a course.")
    //    { }
    //}

    //public class CannotChangeSessionInCourseToCourseErrorResponse : ErrorResponse
    //{
    //    public CannotChangeSessionInCourseToCourseErrorResponse()
    //        : base("Cannot change from a session in a course to a course.")
    //    { }
    //}

    public class BookingUpdateNotSupportedErrorResponse : ErrorResponse
    {
        public BookingUpdateNotSupportedErrorResponse()
            : base("Booking update is not yet supported.")
        { }
    }
}

