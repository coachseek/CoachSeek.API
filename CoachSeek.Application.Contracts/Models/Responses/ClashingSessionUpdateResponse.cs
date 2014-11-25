using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class ClashingSessionUpdateResponse : Response<SessionData>
    {
        public ClashingSessionUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateClashingSessionError() };
        }

        private static ErrorData CreateClashingSessionError()
        {
            return new ErrorData(Resources.ErrorClashingSession);
        }
    }
}