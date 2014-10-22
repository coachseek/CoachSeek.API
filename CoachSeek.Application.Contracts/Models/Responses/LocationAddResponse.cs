using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoLocationAddDataResponse : Response<LocationData>
    {
        public NoLocationAddDataResponse()
        {
            Errors = new List<ErrorData> { CreateNoLocationAddDataError() };
        }

        private static ErrorData CreateNoLocationAddDataError()
        {
            return new ErrorData(Resources.ErrorNoLocationAddData);
        }
    }

    public class InvalidBusinessLocationAddResponse : Response<LocationData>
    {
        public InvalidBusinessLocationAddResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidBusinessError() };
        }

        private static ErrorData CreateInvalidBusinessError()
        {
            return new ErrorData("location.businessId", Resources.ErrorInvalidBusiness);
        }
    }

    public class DuplicateLocationAddResponse : Response<LocationData>
    {
        public DuplicateLocationAddResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateLocationError() };
        }

        private static ErrorData CreateDuplicateLocationError()
        {
            return new ErrorData("location.name", Resources.ErrorDuplicateLocation);
        }
    }
}