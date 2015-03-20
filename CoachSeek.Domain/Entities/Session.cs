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


        protected Session(SessionAddCommand command, CoreData coreData)
        {
            Id = Guid.NewGuid();

            ValidateAndCreate(command, coreData);
        }

        protected Session(Session existingSession, SessionUpdateCommand command, CoreData coreData)
        {
            Id = command.Id;

            ValidateAndCreate(command, coreData);
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


        private void ValidateAndCreate(SessionAddCommand command, CoreData coreData)
        {
            var errors = new ValidationException();

            ValidateAndCreateLocation(coreData.Location, errors);
            ValidateAndCreateCoach(coreData.Coach, errors);
            ValidateAndCreateService(coreData.Service, errors);

            ValidateAndCreateSessionTiming(command.Timing, errors);
            ValidateAndCreateSessionBooking(command.Booking, errors);
            ValidateAndCreateSessionPresentation(command.Presentation, errors);

            ValidateAdditional(command, coreData, errors);

            errors.ThrowIfErrors();
        }


        protected virtual void ValidateAdditional(SessionAddCommand command,
                                                  CoreData coreData,
                                                  ValidationException errors)
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

        protected void ValidateAndCreateSessionTiming(SessionTimingCommand sessionTiming, ValidationException errors)
        {
            try
            {
                _timing = new SessionTiming(sessionTiming);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateSessionBooking(SessionBookingCommand sessionBooking, ValidationException errors)
        {
            try
            {
                _booking = new SessionBooking(sessionBooking);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateSessionPresentation(PresentationCommand sessionPresentation, ValidationException errors)
        {
            try
            {
                _presentation = new SessionPresentation(sessionPresentation);
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
