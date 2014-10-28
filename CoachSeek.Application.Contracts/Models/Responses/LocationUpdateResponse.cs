using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
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