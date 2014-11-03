using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ServiceDefaultsData Defaults
        {
            get { return ServiceDefaults == null ? null : ServiceDefaults.ToData(); }
        }

        public ServiceRepetitionData Repetition
        {
            get { return ServiceRepetition == null ? null : ServiceRepetition.ToData(); }
        }

        private ServiceDefaults ServiceDefaults { get; set; }

        private ServiceRepetition ServiceRepetition { get; set; }


        public Service(Guid id, string name, string description, ServiceDefaultsData defaults, ServiceRepetitionData repetition)
        {
            Id = id;
            Name = name.Trim();
            if (description != null)
                Description = description.Trim();

            var errors = new ValidationException();

            try
            {
                if (defaults != null)
                    ServiceDefaults = new ServiceDefaults(defaults);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }

            try
            {
                if (repetition != null)
                    ServiceRepetition = new ServiceRepetition(repetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }

            errors.ThrowIfErrors();
        }

        public Service(ServiceData data)
            : this(data.Id, data.Name, data.Description, data.Defaults, data.Repetition)
        { }


        public ServiceData ToData()
        {
            return Mapper.Map<Service, ServiceData>(this);
        }
    }
}
