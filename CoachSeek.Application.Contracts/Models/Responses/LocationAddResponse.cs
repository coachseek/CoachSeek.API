using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
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