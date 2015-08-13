using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class InvalidService : SingleErrorException
    {
        public InvalidService(Guid serviceId)
            : base("This service does not exist.",
                   ErrorCodes.ServiceInvalid,
                   serviceId.ToString())
        { }
    }
}