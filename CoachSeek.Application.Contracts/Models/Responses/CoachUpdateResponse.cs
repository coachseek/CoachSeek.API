using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoCoachUpdateDataResponse : Response
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

    public class InvalidBusinessCoachUpdateResponse : Response
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

    public class InvalidCoachUpdateResponse : Response
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

    public class DuplicateCoachUpdateResponse : Response
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


    //public class CoachUpdateResponse : Response
    //{
    //    public CoachUpdateResponse()
    //    { }

    //    public CoachUpdateResponse(Business business)
    //        : base(business)
    //    { }

    //    public CoachUpdateResponse(ValidationException exception)
    //        : base(exception)
    //    { }
    //}
}