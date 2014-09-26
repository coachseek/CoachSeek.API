using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.WebUI.Contracts.Persistence;

namespace CoachSeek.WebUI.Persistence
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