using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoLocationAddDataResponse : Response
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

    public class InvalidBusinessLocationAddResponse : Response
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

    public class DuplicateLocationAddResponse : Response
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


    //public class LocationAddResponse : Response
    //{
    //    public LocationAddResponse()
    //    { }

    //    public LocationAddResponse(Business business) 
    //        : base(business)
    //    { }

    //    public LocationAddResponse(ValidationException exception) 
    //        : base (exception)
    //    { }
    //}
}