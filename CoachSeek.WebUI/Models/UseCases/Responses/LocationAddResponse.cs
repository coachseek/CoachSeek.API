using System.Collections.Generic;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public class NoLocationAddDataResponse : LocationAddResponse
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

    public class InvalidBusinessLocationAddResponse : LocationAddResponse
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

    public class DuplicateLocationAddResponse : LocationAddResponse
    {
        public DuplicateLocationAddResponse()
        {
            Errors = new List<Error> { CreateDuplicateLocationAddError() };
        }

        private static Error CreateDuplicateLocationAddError()
        {
            return new Error((int)ErrorCodes.ErrorDuplicateLocation,
                             Resources.ErrorDuplicateLocation);
        }
    }

    public class LocationAddResponse
    {
        public Business Business { get; private set; }

        public List<Error> Errors { get; protected set; }


        public LocationAddResponse()
        { }

        public LocationAddResponse(Business business)
        {
            Business = business;
        }

        public LocationAddResponse(ValidationException exception)
        {
            Errors = exception.Errors;
        }

        public bool IsSuccessful
        {
            get { return Business != null; }
        }
    }
}