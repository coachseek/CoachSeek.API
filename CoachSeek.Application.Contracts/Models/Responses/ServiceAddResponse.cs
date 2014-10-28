using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class DuplicateServiceAddResponse : Response<ServiceData>
    {
        public DuplicateServiceAddResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateServiceError() };
        }

        private static ErrorData CreateDuplicateServiceError()
        {
            return new ErrorData("location.name", Resources.ErrorDuplicateService);
        }
    }
}