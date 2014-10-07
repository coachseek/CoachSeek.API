using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Properties;
using System.Collections.Generic;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public class NoLocationUpdateDataResponse : LocationUpdateResponse
    {
        public NoLocationUpdateDataResponse()
        {
            Errors = new List<Error> { CreateNoLocationUpdateDataError() };
        }

        private static Error CreateNoLocationUpdateDataError()
        {
            return new Error((int)ErrorCodes.ErrorNoLocationUpdateData,
                             Resources.ErrorNoLocationUpdateData);
        }
    }

    public class InvalidBusinessLocationUpdateResponse : LocationUpdateResponse
    {
        public InvalidBusinessLocationUpdateResponse()
        {
            Errors = new List<Error> { CreateInvalidBusinessError() };
        }

        private static Error CreateInvalidBusinessError()
        {
            return new Error((int)ErrorCodes.ErrorInvalidBusiness,
                             Resources.ErrorInvalidBusiness);
        }
    }

    public class InvalidLocationUpdateResponse : LocationUpdateResponse
    {
        public InvalidLocationUpdateResponse()
        {
            Errors = new List<Error> { CreateInvalidLocationError() };
        }

        private static Error CreateInvalidLocationError()
        {
            return new Error((int)ErrorCodes.ErrorInvalidLocation,
                             Resources.ErrorInvalidLocation);
        }
    }

    public class DuplicateLocationUpdateResponse : LocationUpdateResponse
    {
        public DuplicateLocationUpdateResponse()
        {
            Errors = new List<Error> { CreateDuplicateLocationError() };
        }

        private static Error CreateDuplicateLocationError()
        {
            return new Error((int)ErrorCodes.ErrorDuplicateLocation,
                             Resources.ErrorDuplicateLocation);
        }
    }


    public class LocationUpdateResponse : Response
    {
        public LocationUpdateResponse()
        { }

        public LocationUpdateResponse(Business business)
            : base(business)
        { }

        public LocationUpdateResponse(ValidationException exception)
            : base(exception)
        { }
    }
}