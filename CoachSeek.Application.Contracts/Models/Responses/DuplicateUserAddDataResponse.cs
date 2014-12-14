using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class DuplicateUserAddDataResponse : Response<UserData>
    {
        public DuplicateUserAddDataResponse()
        {
            Errors = new List<ErrorData> { CreateDuplicateUserError() };
        }

        private static ErrorData CreateDuplicateUserError()
        {
            return new ErrorData("registration.registrant.email",
                                 Resources.ErrorUserDuplicateEmail);
        }
    }
}
