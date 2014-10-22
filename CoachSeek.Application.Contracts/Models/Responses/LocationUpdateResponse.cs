using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoLocationUpdateDataResponse : Response<LocationData>
    {
        public NoLocationUpdateDataResponse()
        {
            Errors = new List<ErrorData> { CreateNoLocationUpdateDataError() };
        }

        private static ErrorData CreateNoLocationUpdateDataError()
        {
            return new ErrorData(Resources.ErrorNoLocationUpdateData);
        }
    }

    public class InvalidBusinessLocationUpdateResponse : Response<LocationData>
    {
        public InvalidBusinessLocationUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidBusinessError() };
        }

        private static ErrorData CreateInvalidBusinessError()
        {
            return new ErrorData("location.businessId", Resources.ErrorInvalidBusiness);
        }
    }

    public class InvalidLocationUpdateResponse : Response<LocationData>
    {
        public InvalidLocationUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidLocationError() };
        }

        private static ErrorData CreateInvalidLocationError()
        {
            return new ErrorData("location.id", Resources.ErrorInvalidLocation);
        }
    }

    public class DuplicateLocationUpdateResponse : Response<LocationData>
    {
        public DuplicateLocationUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateLocationError() };
        }

        private static ErrorData CreateDuplicateLocationError()
        {
            return new ErrorData("location.name", Resources.ErrorDuplicateLocation);
        }
    }
}