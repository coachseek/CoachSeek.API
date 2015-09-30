using System.Text.RegularExpressions;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Subdomain
    {
        public string Domain { get; private set; }

        public Subdomain(string domain)
        {
            Validate(domain);
            Domain = domain.ToLowerInvariant();
        }

        private void Validate(string domain)
        {
            if (string.IsNullOrEmpty(domain))
                throw new SubdomainFormatInvalid(domain);
            var matches = Regex.Matches(domain, @"^[a-zA-Z\d][a-zA-Z\d-]*[a-zA-Z\d]$");
            if (matches.Count == 0)
                throw new SubdomainFormatInvalid(domain);
        }
    }
}
