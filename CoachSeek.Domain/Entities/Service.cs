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


        public Service(Guid id, string name, string description)
        {
            Id = id;
            Name = name.Trim();
            if (description != null)
                Description = description.Trim();
        }

        public Service(ServiceData data)
            : this(data.Id, data.Name, data.Description)
        { }


        public ServiceData ToData()
        {
            return Mapper.Map<Service, ServiceData>(this);
        }
    }
}
