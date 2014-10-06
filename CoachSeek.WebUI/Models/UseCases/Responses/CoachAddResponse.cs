using System.Collections.Generic;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public class NoCoachAddDataResponse : CoachAddResponse
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

    public class InvalidBusinessCoachAddResponse : CoachAddResponse
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


    public class CoachAddResponse
    {
        public Business Business { get; private set; }

        public List<Error> Errors { get; protected set; }


        public CoachAddResponse()
        {
        }

        public CoachAddResponse(Business business)
        {
            Business = business;
        }

        public CoachAddResponse(ValidationException exception)
        {
            Errors = exception.Errors;
        }

        public bool IsSuccessful
        {
            get { return Business != null; }
        }
    }
}