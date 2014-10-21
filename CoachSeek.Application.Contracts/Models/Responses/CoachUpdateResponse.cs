using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoCoachUpdateDataResponse : Response<CoachData>
    {
        public NoCoachUpdateDataResponse()
        {
            Errors = new List<ErrorData> { CreateNoCoachUpdateDataError() };
        }

        private static ErrorData CreateNoCoachUpdateDataError()
        {
            return new ErrorData(Resources.ErrorNoCoachUpdateData);
        }
    }

    public class InvalidBusinessCoachUpdateResponse : Response<CoachData>
    {
        public InvalidBusinessCoachUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidBusinessError() };
        }

        private static ErrorData CreateInvalidBusinessError()
        {
            return new ErrorData(Resources.ErrorInvalidBusiness);
        }
    }

    public class InvalidCoachUpdateResponse : Response<CoachData>
    {
        public InvalidCoachUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidCoachError() };
        }

        private static ErrorData CreateInvalidCoachError()
        {
            return new ErrorData(Resources.ErrorInvalidCoach);
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