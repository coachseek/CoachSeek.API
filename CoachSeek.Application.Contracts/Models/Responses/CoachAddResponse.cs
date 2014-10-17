using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoCoachAddDataResponse : Response
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

    public class InvalidBusinessCoachAddResponse : Response
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

    public class DuplicateCoachAddResponse : Response
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


    //public class CoachAddResponse : Response
    //{
    //    public CoachAddResponse()
    //    { }

    //    public CoachAddResponse(Business business)
    //        : base(business)
    //    { }

    //    public CoachAddResponse(ValidationException exception)
    //        : base(exception)
    //    { }
    //}
}