using CoachSeek.Application.Contracts.Properties;

namespace CoachSeek.Application.Contracts.Models
{
    public class InvalidEmailAddressFormatErrorResponse : ErrorResponse
    {
        public InvalidEmailAddressFormatErrorResponse(string field)
            : base(Resources.ErrorInvalidEmailAddressFormat, field)
        { }
    }

    // Temporary Error Responses
}

