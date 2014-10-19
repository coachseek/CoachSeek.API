using System;
using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class BusinessData : IData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }

        public BusinessAdminData Admin { get; set; }
        public IList<LocationData> Locations { get; set; }
        public IList<CoachData> Coaches { get; set; }
    }
}
