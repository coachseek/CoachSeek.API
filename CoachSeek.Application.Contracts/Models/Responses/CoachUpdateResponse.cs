using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class InvalidCoachUpdateResponse : Response<CoachData>
    {
        public InvalidCoachUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidCoachError() };
        }

        private static ErrorData CreateInvalidCoachError()
        {
            return new ErrorData("coach.id", Resources.ErrorInvalidCoach);
        }
    }

    public class DuplicateCoachUpdateResponse : Response<CoachData>
    {
        public DuplicateCoachUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateCoachError() };
        }

        private static ErrorData CreateDuplicateCoachError()
        {
            return new ErrorData(Resources.ErrorDuplicateCoach);
        }
    }
}