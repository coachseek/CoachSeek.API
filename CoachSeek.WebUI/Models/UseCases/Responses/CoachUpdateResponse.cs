﻿using System.Collections.Generic;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public class NoCoachUpdateDataResponse : CoachUpdateResponse
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

    public class InvalidBusinessCoachUpdateResponse : CoachUpdateResponse
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

    public class CoachUpdateResponse : Response
    {
        public CoachUpdateResponse()
        { }

        public CoachUpdateResponse(Business business)
            : base(business)
        { }

        public CoachUpdateResponse(ValidationException exception)
            : base(exception)
        { }
    }
}