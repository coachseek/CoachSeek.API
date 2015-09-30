using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SubdomainDuplicate : SingleErrorException
    {
        public SubdomainDuplicate(string subdomain)
            : base(ErrorCodes.SubdomainDuplicate,
                   string.Format("The subdomain '{0}' already exists.", subdomain),
                   subdomain)
        { }
    }
}
