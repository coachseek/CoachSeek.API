﻿using System.Collections.Generic;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class HardCodedReservedDomainRepository : IReservedDomainRepository
    {
        private static List<string> Domains { get; set; }

        static HardCodedReservedDomainRepository()
        {
            Domains = new List<string>{ "app", "api", "tennis", "football", "underwaterhockey" };
        }

        public bool Contains(string domain)
        {
            return Domains.Contains(domain);
        }
    }
}