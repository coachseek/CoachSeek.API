using System;
using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class BusinessData : IData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }

        public IList<LocationData> Locations { get; set; }
        public IList<CoachData> Coaches { get; set; }
        public IList<ServiceData> Services { get; set; }
        //public IList<SessionData> Sessions { get; set; }
        public IList<SingleSessionData> Sessions { get; set; }
        public IList<RepeatedSessionData> Courses { get; set; }
        public IList<CustomerData> Customers { get; set; }


        public BusinessData()
        {
            Locations = new List<LocationData>();
            Coaches = new List<CoachData>();
            Services = new List<ServiceData>();
            //Sessions = new List<SessionData>();
            Sessions = new List<SingleSessionData>();
            Courses = new List<RepeatedSessionData>();
            Customers = new List<CustomerData>();
        }

        public string GetName()
        {
            return "Business";
        }

        public bool ShouldSerializeLocations() { return Locations.Count > 0; }
        public bool ShouldSerializeCoaches() { return Coaches.Count > 0; }
        public bool ShouldSerializeServices() { return Services.Count > 0; }
        public bool ShouldSerializeSessions() { return Sessions.Count > 0; }
        public bool ShouldSerializeCourses() { return Courses.Count > 0; }
        public bool ShouldSerializeCustomers() { return Customers.Count > 0; }
    }
}
