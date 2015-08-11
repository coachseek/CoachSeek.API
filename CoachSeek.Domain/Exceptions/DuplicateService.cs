using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DuplicateService : SingleErrorException
    {
        public DuplicateService(string serviceName)
            : base(string.Format("Service '{0}' already exists.", serviceName),
                   ErrorCodes.ServiceDuplicate,
                   serviceName)
        { }
    }
}