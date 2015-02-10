using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class InvalidSessionResponse : Response<BookingData>
    {
        public InvalidSessionResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidSessionError() };
        }

        private static ErrorData CreateInvalidSessionError()
        {
            return new ErrorData(Resources.ErrorInvalidSession);
        }
    }
}