using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Session
    {
        private Service _service;
        private Location _location;
        private Coach _coach;
        private SessionTiming _timing;
        private SessionBooking _booking;
        private SessionRepetition _repetition;
        private SessionPricing _pricing;
        private SessionPresentation _presentation;


        public Guid Id { get; private set; }
        public ServiceKeyData Service { get { return _service.ToKeyData(); } }
        public LocationKeyData Location { get { return _location.ToKeyData(); } }
        public CoachKeyData Coach { get { return _coach.ToKeyData(); } }
        public SessionTimingData Timing { get { return _timing.ToData(); } }
        public SessionBookingData Booking { get { return _booking.ToData(); } }
        public PricingData Pricing { get { return _pricing.ToData(); } }
        public RepetitionData Repetition { get { return _repetition.ToData(); } }
        public PresentationData Presentation { get { return _presentation.ToData(); } }


        public Session(SessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Pricing, data.Repetition, data.Presentation)
        { }

        public Session(Guid id, 
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PricingData pricing,
                       RepetitionData repetition,
                       PresentationData presentation)
        {
            Id = id;

            var errors = new ValidationException();

            ValidateAndCreateLocation(location, errors);
            ValidateAndCreateCoach(coach, errors);
            ValidateAndCreateService(service, errors);
            ValidateAndCreateSessionTiming(timing, service.Timing, errors);
            ValidateAndCreateSessionBooking(booking, service.Booking, errors);
            ValidateAndCreateSessionRepetition(repetition, service.Repetition, errors);
            ValidateAndCreateSessionPricing(pricing, service.Pricing, errors);
            ValidateAndCreateSessionPresentation(presentation, service.Presentation, errors);

            errors.ThrowIfErrors();
        }


        public SessionData ToData()
        {
            return Mapper.Map<Session, SessionData>(this);
        }


        private void ValidateAndCreateLocation(LocationData location, ValidationException errors)
        {
            if (location == null)
            {
                errors.Add("The location field is required.", "session.location");
                return;
            }

            try
            {
                _location = new Location(location);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateCoach(CoachData coach, ValidationException errors)
        {
            if (coach == null)
            {
                errors.Add("The coach field is required.", "session.coach");
                return;
            }

            try
            {
                _coach = new Coach(coach);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateService(ServiceData service, ValidationException errors)
        {
            if (service == null)
            {
                errors.Add("The service field is required.", "session.service");
                return;
            }

            try
            {
                _service = new Service(service);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateSessionTiming(SessionTimingData sessionTiming, ServiceTimingData serviceTiming, ValidationException errors)
        {
            try
            {
                _timing = new SessionTiming(sessionTiming, serviceTiming);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateSessionBooking(SessionBookingData sessionBooking, ServiceBookingData serviceBooking, ValidationException errors)
        {
            try
            {
                _booking = new SessionBooking(sessionBooking, serviceBooking);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateSessionRepetition(RepetitionData sessionRepetition, RepetitionData serviceRepetition, ValidationException errors)
        {
            try
            {
                _repetition = new SessionRepetition(sessionRepetition, serviceRepetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateSessionPricing(PricingData sessionPricing, PricingData servicePricing, ValidationException errors)
        {
            try
            {
                _pricing = new SessionPricing(sessionPricing, servicePricing, _repetition);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateAndCreateSessionPresentation(PresentationData sessionPresentation, PresentationData servicePresentation, ValidationException errors)
        {
            try
            {
                _presentation = new SessionPresentation(sessionPresentation, servicePresentation);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
