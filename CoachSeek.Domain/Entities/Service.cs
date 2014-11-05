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
            get { return HasDefaults ? ServiceDefaults.ToData() : null; }
        }

        public ServicePricingData Pricing
        {
            get { return IsPriced ? ServicePricing.ToData() : null; }
        }

        public ServiceRepetitionData Repetition
        {
            get { return IsCourse ? ServiceRepetition.ToData() : null; }
        }

        private ServiceDefaults ServiceDefaults { get; set; }
        private ServicePricing ServicePricing { get; set; }
        private ServiceRepetition ServiceRepetition { get; set; }

        private bool HasDefaults { get { return ServiceDefaults != null; } }
        private bool IsPriced { get { return ServicePricing != null; } }
        private bool IsCourse { get { return ServiceRepetition != null; } }
        private bool HasSessionPrice { get { return (IsPriced && ServicePricing.SessionPrice.HasValue); } }
        private bool HasCoursePrice { get { return (IsPriced && ServicePricing.CoursePrice.HasValue); } }
        private bool IsOpenEnded { get { return (IsCourse && ServiceRepetition.IsOpenEnded); } }

        public Service(Guid id, 
                       string name, 
                       string description, 
                       ServiceDefaultsData defaults, 
                       ServicePricingData pricing, 
                       ServiceRepetitionData repetition)
        {
            Id = id;
            Name = name.Trim();
            if (description != null)
                Description = description.Trim();

            ValidateAndCreateEntities(defaults, pricing, repetition);
            ValidateEntityInteractions();

            if (IsCourse && HasSessionPrice && !HasCoursePrice)
                ServicePricing = new ServicePricing(ServicePricing, Repetition.RepeatTimes);
        }

        private void ValidateAndCreateEntities(ServiceDefaultsData defaults, ServicePricingData pricing, ServiceRepetitionData repetition)
        {
            var errors = new ValidationException();

            ValidateAndCreateDefaults(defaults, errors);
            ValidateAndCreatePricing(pricing, errors);
            ValidateAndCreateRepetition(repetition, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateEntityInteractions()
        {
            var errors = new ValidationException();

            if (HasCoursePrice && (!IsCourse || IsOpenEnded))
                errors.Add("The coursePrice cannot be specified if the service is not for a course or is open-ended.",
                           "service.pricing.coursePrice");

            errors.ThrowIfErrors();
        }

        private void ValidateAndCreateDefaults(ServiceDefaultsData defaults, ValidationException errors)
        {
            try
            {
                if (defaults != null)
                    ServiceDefaults = new ServiceDefaults(defaults);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateRepetition(ServiceRepetitionData repetition, ValidationException errors)
        {
            try
            {
                if (repetition != null)
                    ServiceRepetition = new ServiceRepetition(repetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreatePricing(ServicePricingData pricing, ValidationException errors)
        {
            try
            {
                if (pricing != null)
                    ServicePricing = new ServicePricing(pricing);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        public Service(ServiceData data)
            : this(data.Id, data.Name, data.Description, data.Defaults, data.Pricing, data.Repetition)
        { }


        public ServiceData ToData()
        {
            return Mapper.Map<Service, ServiceData>(this);
        }
    }
}
