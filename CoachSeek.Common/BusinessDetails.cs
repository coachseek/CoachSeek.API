using System;

namespace CoachSeek.Common
{
    public class BusinessDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }


        public BusinessDetails(Guid id, string name, string domain)
        {
            Id = id;
            Name = name;
            Domain = domain;
        }
    }
}
