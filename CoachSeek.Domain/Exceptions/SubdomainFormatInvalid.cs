using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SubdomainFormatInvalid : SingleErrorException
    {
        public SubdomainFormatInvalid(string subdomain)
            : base(ErrorCodes.SubdomainFormatInvalid,
                   string.Format("The subdomain '{0}' is not in a valid format.", subdomain),
                   subdomain)
        { }
    }
}
