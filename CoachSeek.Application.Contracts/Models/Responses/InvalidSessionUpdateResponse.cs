using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class InvalidSessionUpdateResponse : Response<SessionData>
    {
        public InvalidSessionUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidSessionError() };
        }

        private static ErrorData CreateInvalidSessionError()
        {
            return new ErrorData("session.id", Resources.ErrorInvalidSession);
        }
    }
}
