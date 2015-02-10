using CoachSeek.Application.Contracts.Properties;

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
        public ClashingSessionErrorResponse()
            : base(Resources.ErrorClashingSession)
        { }
    }
}

