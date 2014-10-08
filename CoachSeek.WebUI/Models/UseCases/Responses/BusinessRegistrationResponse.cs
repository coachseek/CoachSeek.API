using System.Collections.Generic;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public class NoBusinessRegistrationDataResponse : BusinessRegistrationResponse
    {
        public NoBusinessRegistrationDataResponse()
        {
            Errors = new List<Error> { CreateNoBusinessRegistrationDataError() };
        }

        private static Error CreateNoBusinessRegistrationDataError()
        {
            return new Error((int)ErrorCodes.ErrorNoBusinessRegistrationData,
                             Resources.ErrorNoBusinessRegistrationData);
        }
    }

    public class BusinessRegistrationResponse
    {
        public Business Business { get; private set; }

        public List<Error> Errors { get; protected set; }


        public BusinessRegistrationResponse()
        { }

        public BusinessRegistrationResponse(Business business)
        {
            Business = business;
        }

        public BusinessRegistrationResponse(ValidationException exception)
        {
            Errors = exception.Errors;
        }

        public bool IsSuccessful
        {
            get { return Business != null; }
        }
    }
}