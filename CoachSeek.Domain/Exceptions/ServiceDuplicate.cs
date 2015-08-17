using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class ServiceDuplicate : SingleErrorException
    {
        public ServiceDuplicate(string serviceName)
            : base(ErrorCodes.ServiceDuplicate,
                   string.Format("Service '{0}' already exists.", serviceName), 
                   serviceName)
        { }
    }
}