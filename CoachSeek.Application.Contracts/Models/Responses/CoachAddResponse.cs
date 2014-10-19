using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoCoachAddDataResponse : Response<CoachData>
    {
        public NoCoachAddDataResponse()
        {
            Errors = new List<Error> { CreateNoCoachAddDataError() };
        }

        private static Error CreateNoCoachAddDataError()
        {
            return new Error((int)ErrorCodes.ErrorNoCoachAddData,
                             Resources.ErrorNoCoachAddData);
        }
    }

    public class InvalidBusinessCoachAddResponse : Response<CoachData>
    {
        public InvalidBusinessCoachAddResponse()
        {
            Errors = new List<Error> { CreateInvalidBusinessError() };
        }

        private static Error CreateInvalidBusinessError()
        {
            return new Error((int)ErrorCodes.ErrorInvalidBusiness,
                             Resources.ErrorInvalidBusiness);
        }
    }

    public class DuplicateCoachAddResponse : Response<CoachData>
    {
        public DuplicateCoachAddResponse()
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