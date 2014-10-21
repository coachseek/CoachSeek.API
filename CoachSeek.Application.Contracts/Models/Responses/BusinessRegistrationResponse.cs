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
            Errors = new List<ErrorData> { CreateNoBusinessRegistrationDataError() };
        }

        private static ErrorData CreateNoBusinessRegistrationDataError()
        {
            return new ErrorData(Resources.ErrorNoBusinessRegistrationData);
        }
    }
    public class DuplicateBusinessAdminBusinessRegistrationResponse : Response<BusinessData>
    {
        public DuplicateBusinessAdminBusinessRegistrationResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateBusinessAdminError() };
        }

        private static ErrorData CreateDuplicateBusinessAdminError()
        {
            return new ErrorData("registration.registrant.email",
                                 Resources.ErrorBusinessAdminDuplicateEmail);
        }
    }
}