using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoDataResponse<TData> : Response<TData> where TData : class, IData, new()
    {
        public NoDataResponse()
        {
            Errors = new List<ErrorData> { CreateNoDataError() };
        }

        private static ErrorData CreateNoDataError()
        {
            return new ErrorData(string.Format(Resources.ErrorNoData, new TData().GetName()));
        }
    }
}
