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

    public class DuplicateBusinessAdminErrorResponse : ErrorResponse
    {
        public DuplicateBusinessAdminErrorResponse()
            : base(Resources.ErrorBusinessAdminDuplicateEmail, "registration.registrant.email")
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
            : base(Resources.ErrorInvalidSession, "session.id")
        { }

        public InvalidSessionErrorResponse(string field)
            : base(Resources.ErrorInvalidSession, field)
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

    public class ClashingSessionErrorResponse : ErrorResponse
    {
        public ClashingSessionErrorResponse(ClashingSession clashingSession)
            : base(Resources.ErrorClashingSession, null, string.Format("Clashing session: {0}; SessionId = {{{1}}}", 
                                                                       clashingSession.Session.Name, clashingSession.Session.Id))
        { }
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

    public class CannotChangeStandaloneSessionToCourseErrorResponse : ErrorResponse
    {
        public CannotChangeStandaloneSessionToCourseErrorResponse()
            : base("Cannot change from a standalone session to a course.")
        { }
    }

    public class CannotChangeSessionInCourseToCourseErrorResponse : ErrorResponse
    {
        public CannotChangeSessionInCourseToCourseErrorResponse()
            : base("Cannot change from a session in a course to a course.")
        { }
    }
}

