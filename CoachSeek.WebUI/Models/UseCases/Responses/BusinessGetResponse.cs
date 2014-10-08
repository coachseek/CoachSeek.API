using System.Collections.Generic;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public class NoDomainBusinessGetResponse : BusinessGetResponse
    {
        public NoDomainBusinessGetResponse()
        {
            Errors = new List<Error> { CreateNoDomainBusinessGetError() };
        }

        private static Error CreateNoDomainBusinessGetError()
        {
            return new Error((int)ErrorCodes.ErrorNoBusinessDomain,
                             Resources.ErrorNoBusinessDomain);
        }
    }

    public class BusinessGetResponse : Response
    {
        public BusinessGetResponse()
        { }

        public BusinessGetResponse(Business business)
            : base(business)
        { }

        public BusinessGetResponse(ValidationException exception)
            : base(exception)
        { }
    }
}