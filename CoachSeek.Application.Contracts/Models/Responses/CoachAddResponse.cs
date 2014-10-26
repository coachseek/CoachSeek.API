using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoCoachAddDataResponse : Response<CoachData>
    {
        public NoCoachAddDataResponse()
        {
            Errors = new List<ErrorData> { CreateNoCoachAddDataError() };
        }

        private static ErrorData CreateNoCoachAddDataError()
        {
            return new ErrorData(Resources.ErrorNoCoachAddData);
        }
    }

    public class InvalidBusinessCoachAddResponse : Response<CoachData>
    {
        public InvalidBusinessCoachAddResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidBusinessError() };
        }

        private static ErrorData CreateInvalidBusinessError()
        {
            return new ErrorData("coach.businessId", Resources.ErrorInvalidBusiness);
        }
    }

    public class DuplicateCoachAddResponse : Response<CoachData>
    {
        public DuplicateCoachAddResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateCoachError() };
        }

        private static ErrorData CreateDuplicateCoachError()
        {
            return new ErrorData(Resources.ErrorDuplicateCoach);
        }
    }
}