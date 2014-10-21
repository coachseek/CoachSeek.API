using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoDomainBusinessGetResponse : Response<BusinessData>
    {
        public NoDomainBusinessGetResponse()
        {
            Errors = new List<ErrorData> { CreateNoDomainBusinessGetError() };
        }

        private static ErrorData CreateNoDomainBusinessGetError()
        {
            return new ErrorData(Resources.ErrorNoBusinessDomain);
        }
    }

    public class NotFoundBusinessGetResponse : Response<BusinessData>
    {
        public NotFoundBusinessGetResponse()
        {
            Errors = new List<ErrorData> { CreateNoDomainBusinessGetError() };
        }

        private static ErrorData CreateNoDomainBusinessGetError()
        {
            return new ErrorData(Resources.ErrorNoBusinessDomain);
        }
    }
}