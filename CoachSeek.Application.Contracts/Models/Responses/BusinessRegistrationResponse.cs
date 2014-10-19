using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoBusinessRegistrationDataResponse : Response<BusinessData>
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
    public class DuplicateBusinessAdminBusinessRegistrationResponse : Response<BusinessData>
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
}