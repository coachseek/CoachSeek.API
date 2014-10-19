using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoLocationAddDataResponse : Response<LocationData>
    {
        public NoLocationAddDataResponse()
        {
            Errors = new List<Error> { CreateNoLocationAddDataError() };
        }

        private static Error CreateNoLocationAddDataError()
        {
            return new Error((int)ErrorCodes.ErrorNoLocationAddData,
                             Resources.ErrorNoLocationAddData);
        }
    }

    public class InvalidBusinessLocationAddResponse : Response<LocationData>
    {
        public InvalidBusinessLocationAddResponse()
        {
            Errors = new List<Error> { CreateInvalidBusinessError() };
        }

        private static Error CreateInvalidBusinessError()
        {
            return new Error((int)ErrorCodes.ErrorInvalidBusiness,
                             Resources.ErrorInvalidBusiness);
        }
    }

    public class DuplicateLocationAddResponse : Response<LocationData>
    {
        public DuplicateLocationAddResponse()
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