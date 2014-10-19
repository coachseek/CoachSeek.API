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
            Errors = new List<Error> { CreateNoCoachUpdateDataError() };
        }

        private static Error CreateNoCoachUpdateDataError()
        {
            return new Error((int)ErrorCodes.ErrorNoCoachUpdateData,
                             Resources.ErrorNoCoachUpdateData);
        }
    }

    public class InvalidBusinessCoachUpdateResponse : Response<CoachData>
    {
        public InvalidBusinessCoachUpdateResponse()
        {
            Errors = new List<Error> { CreateInvalidBusinessError() };
        }

        private static Error CreateInvalidBusinessError()
        {
            return new Error((int)ErrorCodes.ErrorInvalidBusiness,
                             Resources.ErrorInvalidBusiness);
        }
    }

    public class InvalidCoachUpdateResponse : Response<CoachData>
    {
        public InvalidCoachUpdateResponse()
        {
            Errors = new List<Error> { CreateInvalidCoachError() };
        }

        private static Error CreateInvalidCoachError()
        {
            return new Error((int)ErrorCodes.ErrorInvalidCoach,
                             Resources.ErrorInvalidCoach);
        }
    }

    public class DuplicateCoachUpdateResponse : Response<CoachData>
    {
        public DuplicateCoachUpdateResponse()
        {
            Errors = new List<Error> { CreateDuplicateCoachError() };
        }

        private static Error CreateDuplicateCoachError()
        {
            return new Error((int)ErrorCodes.ErrorDuplicateCoach,
                             Resources.ErrorDuplicateCoach);
        }
    }
}