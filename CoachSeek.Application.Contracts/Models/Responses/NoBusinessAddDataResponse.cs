using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoBusinessAddDataResponse : Response<BusinessData>
    {
        public NoBusinessAddDataResponse()
        {
            Errors = new List<ErrorData> { CreateNoBusinessAddDataError() };
        }

        private static ErrorData CreateNoBusinessAddDataError()
        {
            return new ErrorData(Resources.ErrorNoBusinessAddData);
        }
    }
}
