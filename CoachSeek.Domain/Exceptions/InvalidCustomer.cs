using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class InvalidCustomer : SingleErrorException
    {
        public InvalidCustomer(Guid customerId)
            : base("This customer does not exist.",
                   ErrorCodes.CustomerInvalid,
                   customerId.ToString())
        { }
    }
}