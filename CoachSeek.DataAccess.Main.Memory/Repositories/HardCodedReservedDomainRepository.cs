using System.Collections.Generic;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Repositories
{
    public class HardCodedReservedDomainRepository : IReservedDomainRepository
    {
        private static List<string> Domains { get; set; }

        static HardCodedReservedDomainRepository()
        {
            Domains = new List<string>{ "tennis", "football", "underwaterhockey" };
        }

        public bool Contains(string domain)
        {
            return Domains.Contains(domain);
        }
    }
}