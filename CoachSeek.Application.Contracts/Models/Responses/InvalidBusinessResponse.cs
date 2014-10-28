using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class InvalidBusinessResponse<TData> : Response<TData> where TData : class, IData, new()
    {
        public InvalidBusinessResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidBusinessError() };
        }

        private static ErrorData CreateInvalidBusinessError()
        {
            return new ErrorData(new TData().GetBusinessIdPath(), Resources.ErrorInvalidBusiness);
        }
    }
}
