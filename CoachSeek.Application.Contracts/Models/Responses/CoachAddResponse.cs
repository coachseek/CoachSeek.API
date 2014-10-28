using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class DuplicateCoachAddResponse : Response<CoachData>
    {
        public DuplicateCoachAddResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateCoachError() };
        }

        private static ErrorData CreateDuplicateCoachError()
        {
            return new ErrorData(Resources.ErrorDuplicateCoach);
        }
    }
}