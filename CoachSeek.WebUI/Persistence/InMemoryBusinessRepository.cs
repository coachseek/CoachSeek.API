using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Models;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.WebUI.Persistence
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasAddCalled; 

        private static List<Business> Businesses { get; set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<Business>();
        }


        public Business Add(Business business)
        {
            WasAddCalled = true;

            business.Id = NextId;
            Businesses.Add(business);
            return business;
        }

        public Business GetByDomain(string domain)
        {
            return Businesses.FirstOrDefault(x => x.Domain == domain);
        }


        private static int MaxId
        {
            get 
            {
                if (Businesses.Any())
                    return Businesses.MaxBy(x => x.Id).Id;
                return 0;
            }
        }

        private static int NextId
        {
            get { return MaxId + 1; }
        }
    }
}