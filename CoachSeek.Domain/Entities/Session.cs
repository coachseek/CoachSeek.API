using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public abstract class Session
    {
        protected Service _service;
        protected Location _location;
        protected Coach _coach;
        protected SessionTiming _timing;
        protected SessionBooking _booking;
        protected SessionPresentation _presentation;


        public Guid Id { get; protected set; }
        public string Name { get { return FormatSessionName(); } }
        public ServiceKeyData Service { get { return _service.ToKeyData(); } }
        public LocationKeyData Location { get { return _location.ToKeyData(); } }
        public CoachKeyData Coach { get { return _coach.ToKeyData(); } }
        public SessionTimingData Timing { get { return _timing.ToData(); } }
        public SessionBookingData Booking { get { return _booking.ToData(); } }
        public PresentationData Presentation { get { return _presentation.ToData(); } }


        protected Session(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            Id = Guid.NewGuid();

            ValidateAndCreate(location, coach, service, command);
        }

        protected Session(SessionUpdateCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            Id = command.Id;

            ValidateAndCreate(location, coach, service, command);
        }

        protected Session(SessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation)
        { }

        protected Session(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation)
        {
            Id = id;

            _location = new Location(location);
            _coach = new Coach(coach);
            _service = new Service(service);
            _timing = new SessionTiming(timing);
            _booking = new SessionBooking(booking);
            _presentation = new SessionPresentation(presentation);
        }


        public abstract bool IsOverlapping(Session otherSession);

        //public abstract SessionData ToData();

        public SessionKeyData ToKeyData()
        {
            return Mapper.Map<Session, SessionKeyData>(this);
        }


        private void ValidateAndCreate(LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionAddCommand command)
        {
            var errors = new ValidationException();

            ValidateAndCreateLocation(location, errors);
            ValidateAndCreateCoach(coach, errors);
            ValidateAndCreateService(service, errors);
            ValidateAndCreateSessionTiming(command.Timing, service.Timing, errors);
            ValidateAndCreateSessionBooking(command.Booking, service.Booking, errors);
            ValidateAndCreateSessionPresentation(command.Presentation, service.Presentation, errors);

            ValidateAdditional(errors, location, coach, service, command);

            errors.ThrowIfErrors();
        }


        protected virtual void ValidateAdditional(ValidationException errors,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionAddCommand command)
        { }


        protected void ValidateAndCreateLocation(LocationData location, ValidationException errors)
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

        protected void ValidateAndCreateCoach(CoachData coach, ValidationException errors)
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

        protected void ValidateAndCreateService(ServiceData service, ValidationException errors)
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

        protected void ValidateAndCreateSessionTiming(SessionTimingCommand sessionTiming, ServiceTimingData serviceTiming, ValidationException errors)
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

        protected void ValidateAndCreateSessionBooking(SessionBookingCommand sessionBooking, ServiceBookingData serviceBooking, ValidationException errors)
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

        protected void ValidateAndCreateSessionPresentation(PresentationCommand sessionPresentation, PresentationData servicePresentation, ValidationException errors)
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

        protected bool IsNullOrSameSession(Session otherSession)
        {
            return (otherSession == null || otherSession.Equals(this) || otherSession.Id == Id);
        }

        protected virtual string FormatSessionName()
        {
            return string.Format("{0} at {1} with {2} on {3} at {4}", 
                                 Service.Name, 
                                 Location.Name, 
                                 Coach.Name, 
                                 Timing.StartDate, 
                                 Timing.StartTime);
        }
    }
}
