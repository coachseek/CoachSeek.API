using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Factories;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.WebUI.Persistence
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasSaveNewBusinessCalled;
        public bool WasSaveBusinessCalled; 

        private static List<DbBusiness> Businesses { get; set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();
        }


        public Business Save(NewBusiness newBusiness)
        {
            WasSaveNewBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(newBusiness);

            Businesses.Add(dbBusiness);
            return newBusiness;
        }

        public Business Save(Business business)
        {
            WasSaveBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(business);
            var existingBusiness = Businesses.Single(x => x.Id == dbBusiness.Id);
            var existingIndex = Businesses.IndexOf(existingBusiness);
            Businesses[existingIndex] = dbBusiness;
            var updateBusiness = Businesses.Single(x => x.Id == dbBusiness.Id);
            return BusinessFactory.Create(updateBusiness);
        }
        
        public Business Add(Business business)
        {
            WasSaveBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(business);

            Businesses.Add(dbBusiness);
            return business;
        }

        public Business Get(Identifier id)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Id == id.Id);
            return BusinessFactory.Create(dbBusiness);
        }

        public Business GetByDomain(string domain)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Domain == domain);
            return BusinessFactory.Create(dbBusiness);
        }

        public Business GetByAdminEmail(string adminEmail)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Admin.Email == adminEmail);
            return BusinessFactory.Create(dbBusiness);
        }


        //private static int MaxId
        //{
        //    get 
        //    {
        //        if (Businesses.Any())
        //            return Businesses.MaxBy(x => x.Identifier).Identifier;
        //        return 0;
        //    }
        //}

        //private static int NextId
        //{
        //    get { return MaxId + 1; }
        //}
    }
}