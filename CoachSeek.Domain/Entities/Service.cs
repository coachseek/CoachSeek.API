using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ServiceDefaultsData Defaults
        {
            get
            {
                return ServiceDefaults == null ? null : ServiceDefaults.ToData();
            }
        }

        private ServiceDefaults ServiceDefaults { get; set; }


        public Service(Guid id, string name, string description, ServiceDefaultsData defaults)
        {
            Id = id;
            Name = name.Trim();
            if (description != null)
                Description = description.Trim();
            if (defaults != null)
                ServiceDefaults = new ServiceDefaults(defaults);
        }

        public Service(ServiceData data)
            : this(data.Id, data.Name, data.Description, data.Defaults)
        { }


        public ServiceData ToData()
        {
            return Mapper.Map<Service, ServiceData>(this);
        }
    }
}
