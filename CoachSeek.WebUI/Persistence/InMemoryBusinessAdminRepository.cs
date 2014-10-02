//using System.Collections.Generic;
//using System.Linq;
//using CoachSeek.WebUI.Contracts.Persistence;
//using CoachSeek.WebUI.Models;
//using MoreLinq;

//namespace CoachSeek.WebUI.Persistence
//{
//    public class InMemoryBusinessAdminRepository : IBusinessAdminRepository
//    {
//        // Spy behaviour is included
//        public bool WasAddCalled; 
        
//        private static List<BusinessAdmin> Admins { get; set; }


//        static InMemoryBusinessAdminRepository()
//        {
//            Admins = new List<BusinessAdmin>();
//        }


//        public BusinessAdmin Add(BusinessAdmin admin)
//        {
//            WasAddCalled = true;

//            admin.Identifier = NextId;
//            Admins.Add(admin);
//            return admin;
//        }

//        public BusinessAdmin GetByEmail(string email)
//        {
//            return Admins.FirstOrDefault(x => x.Email == email);
//        }


//        private static int MaxId
//        {
//            get
//            {
//                if (Admins.Any())
//                    return Admins.MaxBy(x => x.Identifier).Identifier;
//                return 0;
//            }
//        }

//        private static int NextId
//        {
//            get { return MaxId + 1; }
//        }
//    }
//}