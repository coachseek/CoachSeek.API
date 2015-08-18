using System;
using AutoMapper;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ServiceTimingData Timing
        {
            get { return ServiceTiming != null ? ServiceTiming.ToData() : null; }
        }

        public ServiceBookingData Booking
        {
            get { return ServiceBooking != null ? ServiceBooking.ToData() : null; }
        }

        public RepeatedSessionPricingData Pricing
        {
            get { return IsPriced ? ServicePricing.ToData() : null; }
        }

        public RepetitionData Repetition
        {
            get { return ServiceRepetition.ToData(); }
        }

        public PresentationData Presentation
        {
            get { return ServicePresentation != null ? ServicePresentation.ToData() : null; }
        }

        private ServiceTiming ServiceTiming { get; set; }
        private ServiceBooking ServiceBooking { get; set; }
        private ServicePricing ServicePricing { get; set; }
        private ServiceRepetition ServiceRepetition { get; set; }
        private ServicePresentation ServicePresentation { get; set; }

        private bool IsPriced { get { return ServicePricing != null; } }
        private bool IsCourse { get { return ServiceRepetition.IsCourse; } }
        private bool IsSingleSession { get { return ServiceRepetition.IsSingleSession; } }
        private bool HasSessionPrice { get { return (IsPriced && ServicePricing.SessionPrice.HasValue); } }
        private bool HasCoursePrice { get { return (IsPriced && ServicePricing.CoursePrice.HasValue); } }


        public Service(ServiceAddCommand command)
            : this(Guid.NewGuid(), 
                   command.Name, 
                   command.Description, 
                   command.Timing, 
                   command.Booking, 
                   command.Pricing, 
                   command.Repetition, 
                   command.Presentation)
        { }

        public Service(ServiceUpdateCommand command)
            : this(command.Id, 
                   command.Name, 
                   command.Description, 
                   command.Timing, 
                   command.Booking, 
                   command.Pricing, 
                   command.Repetition, 
                   command.Presentation)
        { }

        public Service(ServiceData data)
            : this(data.Id, 
                   data.Name, 
                   data.Description, 
                   data.Timing, 
                   data.Booking, 
                   data.Pricing, 
                   data.Repetition, 
                   data.Presentation)
        { }

        public Service(Guid id,
                       string name,
                       string description,
                       ServiceTimingCommand timing,
                       ServiceBookingCommand booking,
                       PricingCommand pricing,
                       RepetitionCommand repetition,
                       PresentationCommand presentation)
        {
            Id = id;
            Name = name.Trim();
            if (description.IsExisting())
                Description = description.Trim();

            ValidateAndCreateEntities(timing, booking, pricing, repetition, presentation);
            ValidateEntityInteractions();
        }

        public Service(Guid id, 
                       string name, 
                       string description,
                       ServiceTimingData timing,
                       ServiceBookingData booking,
                       RepeatedSessionPricingData pricing, 
                       RepetitionData repetition,
                       PresentationData presentation)
        {
            Id = id;
            Name = name;
            Description = description;

            CreateEntities(timing, booking, pricing, repetition, presentation);
        }

        public ServiceData ToData()
        {
            return Mapper.Map<Service, ServiceData>(this);
        }

        public ServiceKeyData ToKeyData()
        {
            return Mapper.Map<Service, ServiceKeyData>(this);
        }


        private void CreateEntities(ServiceTimingData timing, 
                                    ServiceBookingData booking,
                                    RepeatedSessionPricingData pricing, 
                                    RepetitionData repetition,
                                    PresentationData presentation)
        {
            CreateTiming(timing);
            CreateBooking(booking);
            CreateRepetition(repetition);   // CreatePricing depends on CreateRepetition having been called first.
            CreatePricing(pricing);
            CreatePresentation(presentation);
        }

        private void ValidateAndCreateEntities(ServiceTimingCommand timing,
                                               ServiceBookingCommand booking,
                                               PricingCommand pricing,
                                               RepetitionCommand repetition,
                                               PresentationCommand presentation)
        {
            var errors = new ValidationException();

            ValidateAndCreateTiming(timing, errors);
            ValidateAndCreateBooking(booking, errors);
            ValidateAndCreateRepetition(repetition, errors);    // ValidateAndCreatePricing depends on ValidateAndCreateRepetition having been called first.
            ValidateAndCreatePricing(pricing, errors);
            ValidateAndCreatePresentation(presentation, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateEntityInteractions()
        {
            var errors = new ValidationException();

            if (IsSingleSession && HasCoursePrice)
                errors.Add(new ServiceForStandaloneSessionMustHaveNoCoursePrice());

            errors.ThrowIfErrors();
        }

        private void ValidateAndCreateTiming(ServiceTimingCommand timing, ValidationException errors)
        {
            try
            {
                if (timing != null)
                    ServiceTiming = new ServiceTiming(timing);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateBooking(ServiceBookingCommand booking, ValidationException errors)
        {
            try
            {
                if (booking != null)
                    ServiceBooking = new ServiceBooking(booking);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateRepetition(RepetitionCommand repetition, ValidationException errors)
        {
            try
            {
                // Data input validation ensures that the RepetitionCommand is always present.
                ServiceRepetition = new ServiceRepetition(repetition);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreatePricing(PricingCommand pricing, ValidationException errors)
        {
            try
            {
                if (pricing != null && ServiceRepetition != null)
                    ServicePricing = new ServicePricing(pricing, ServiceRepetition);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreatePresentation(PresentationCommand presentation, ValidationException errors)
        {
            try
            {
                ServicePresentation = new ServicePresentation(presentation);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void CreateTiming(ServiceTimingData timing)
        {
            if (timing != null)
                ServiceTiming = new ServiceTiming(timing);
        }

        private void CreateBooking(ServiceBookingData booking)
        {
            if (booking != null)
                ServiceBooking = new ServiceBooking(booking);
        }

        private void CreateRepetition(RepetitionData repetition)
        {
            ServiceRepetition = new ServiceRepetition(repetition);
        }

        private void CreatePricing(RepeatedSessionPricingData pricing)
        {
            if (pricing != null)
                ServicePricing = new ServicePricing(pricing);
        }

        private void CreatePresentation(PresentationData presentation)
        {
            if (presentation != null)
                ServicePresentation = new ServicePresentation(presentation);
        }
    }
}
