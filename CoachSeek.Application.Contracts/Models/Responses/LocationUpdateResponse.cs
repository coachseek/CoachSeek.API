using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoLocationUpdateDataResponse : Response<LocationData>
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

    public class InvalidBusinessLocationUpdateResponse : Response<LocationData>
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

    public class InvalidLocationUpdateResponse : Response<LocationData>
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

    public class DuplicateLocationUpdateResponse : Response<LocationData>
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
}