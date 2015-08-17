using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class ServiceInvalid : SingleErrorException
    {
        public ServiceInvalid(Guid serviceId)
            : base(ErrorCodes.ServiceInvalid, 
                   "This service does not exist.",
                   serviceId.ToString())
        { }
    }
}