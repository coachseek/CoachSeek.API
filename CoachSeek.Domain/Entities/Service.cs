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

        public ServiceTimingData Timing
        {
            get { return ServiceTiming != null ? ServiceTiming.ToData() : null; }
        }

        public ServiceBookingData Booking
        {
            get { return ServiceBooking != null ? ServiceBooking.ToData() : null; }
        }

        public PricingData Pricing
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
        private bool IsCourse { get { return ServiceRepetition.IsRepeatingSession; } }
        private bool IsSingleSession { get { return ServiceRepetition.IsSingleSession; } }
        private bool HasSessionPrice { get { return (IsPriced && ServicePricing.SessionPrice.HasValue); } }
        private bool HasCoursePrice { get { return (IsPriced && ServicePricing.CoursePrice.HasValue); } }
        private bool IsOpenEnded { get { return (IsCourse && ServiceRepetition.IsOpenEnded); } }


        public Service(ServiceData data)
            : this(data.Id, data.Name, data.Description, data.Timing, data.Booking, data.Pricing, data.Repetition, data.Presentation)
        { }

        public Service(Guid id, 
                       string name, 
                       string description,
                       ServiceTimingData timing,
                       ServiceBookingData booking,
                       PricingData pricing, 
                       RepetitionData repetition,
                       PresentationData presentation)
        {
            Id = id;
            Name = name.Trim();
            if (description != null)
                Description = description.Trim();

            ValidateAndCreateEntities(timing, booking, pricing, repetition, presentation);
            ValidateEntityInteractions();

            if (IsCourse && HasSessionPrice && !HasCoursePrice)
                ServicePricing = new ServicePricing(ServicePricing, Repetition.RepeatTimes);
        }


        public ServiceData ToData()
        {
            return Mapper.Map<Service, ServiceData>(this);
        }

        public ServiceKeyData ToKeyData()
        {
            return Mapper.Map<Service, ServiceKeyData>(this);
        }


        private void ValidateAndCreateEntities(ServiceTimingData timing, 
                                               ServiceBookingData booking, 
                                               PricingData pricing, 
                                               RepetitionData repetition,
                                               PresentationData presentation)
        {
            var errors = new ValidationException();

            ValidateAndCreateTiming(timing, errors);
            ValidateAndCreateBooking(booking, errors);
            ValidateAndCreatePricing(pricing, errors);
            ValidateAndCreateRepetition(repetition, errors);
            ValidateAndCreatePresentation(presentation, errors);

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

        private void ValidateAndCreateTiming(ServiceTimingData timing, ValidationException errors)
        {
            try
            {
                if (timing != null)
                    ServiceTiming = new ServiceTiming(timing);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateBooking(ServiceBookingData booking, ValidationException errors)
        {
            try
            {
                if (booking != null)
                    ServiceBooking = new ServiceBooking(booking);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateRepetition(RepetitionData repetition, ValidationException errors)
        {
            if (repetition == null)
            {
                errors.Add("The repetition field is required.", "service.repetition");
                return;
            }

            try
            {
                ServiceRepetition = new ServiceRepetition(repetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreatePricing(PricingData pricing, ValidationException errors)
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

        private void ValidateAndCreatePresentation(PresentationData presentation, ValidationException errors)
        {
            try
            {
                if (presentation != null)
                    ServicePresentation = new ServicePresentation(presentation);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
