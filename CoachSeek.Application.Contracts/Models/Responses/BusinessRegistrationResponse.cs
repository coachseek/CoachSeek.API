using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Common;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
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
    public class DuplicateBusinessAdminBusinessRegistrationResponse : BusinessRegistrationResponse
    {
        public DuplicateBusinessAdminBusinessRegistrationResponse()
        {
            Errors = new List<Error> { CreateDuplicateBusinessAdminError() };
        }

        private static Error CreateDuplicateBusinessAdminError()
        {
            return new Error((int)ErrorCodes.ErrorBusinessAdminDuplicateEmail,
                             Resources.ErrorBusinessAdminDuplicateEmail,
                             FormField.Email);
        }
    }

    public class BusinessRegistrationResponse : Response
    {
        public BusinessRegistrationResponse()
        { }

        public BusinessRegistrationResponse(Business business)
            : base(business)
        { }

        public BusinessRegistrationResponse(ValidationException exception)
            : base(exception)
        { }
    }
}