using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class InvalidServiceUpdateResponse : Response<ServiceData>
    {
        public InvalidServiceUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidServiceError() };
        }

        private static ErrorData CreateInvalidServiceError()
        {
            return new ErrorData("service.id", Resources.ErrorInvalidService);
        }
    }

    public class DuplicateServiceUpdateResponse : Response<ServiceData>
    {
        public DuplicateServiceUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateServiceError() };
        }

        private static ErrorData CreateDuplicateServiceError()
        {
            return new ErrorData("service.name", Resources.ErrorDuplicateService);
        }
    }
}