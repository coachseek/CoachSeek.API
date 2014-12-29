using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
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