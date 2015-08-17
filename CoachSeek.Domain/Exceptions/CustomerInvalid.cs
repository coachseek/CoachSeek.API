using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomerInvalid : SingleErrorException
    {
        public CustomerInvalid(Guid customerId)
            : base(ErrorCodes.CustomerInvalid, 
                   "This customer does not exist.",
                   customerId.ToString())
        { }
    }
}