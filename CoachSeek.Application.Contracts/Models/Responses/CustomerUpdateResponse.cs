using System.Collections.Generic;
using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class InvalidCustomerUpdateResponse : Response<CustomerData>
    {
        public InvalidCustomerUpdateResponse()
        {
            Errors = new List<ErrorData> { CreateInvalidCustomerError() };
        }

        private static ErrorData CreateInvalidCustomerError()
        {
            return new ErrorData("customer.id", Resources.ErrorInvalidCustomer);
        }
    }

    //public class DuplicateCoachUpdateResponse : Response<CoachData>
    //{
    //    public DuplicateCoachUpdateResponse()
    //    {
    //        Errors = new List<ErrorData> { CreateDuplicateCoachError() };
    //    }

    //    private static ErrorData CreateDuplicateCoachError()
    //    {
    //        return new ErrorData(Resources.ErrorDuplicateCoach);
    //    }
    //}
}